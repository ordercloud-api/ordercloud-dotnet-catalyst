using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class WebhookActionSelector : IActionSelector
	{
		private readonly ActionSelector _defaultSelector;

		public WebhookActionSelector(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, ActionConstraintCache actionConstraintCache, ILoggerFactory loggerFactory)
		{
			_defaultSelector = new ActionSelector(actionDescriptorCollectionProvider, actionConstraintCache, loggerFactory);
		}

		public IReadOnlyList<ActionDescriptor> SelectCandidates(RouteContext context)
		{
			return _defaultSelector.SelectCandidates(context);
		}

		public ActionDescriptor SelectBestCandidate(RouteContext context, IReadOnlyList<ActionDescriptor> candidates)
		{
			try
			{
				return _defaultSelector.SelectBestCandidate(context, candidates);
			}
			catch (AmbiguousActionException)
			{
				if (DisambiguateWebhookAction(context, candidates, out var result))
					return result;
				throw;
			}
		}

		private bool DisambiguateWebhookAction(RouteContext context, IReadOnlyList<ActionDescriptor> candidates, out ActionDescriptor result)
		{
			result = null;

			var webhookHandlers = (
				from a in candidates
				where a.Parameters.Count == 1
				let p = a.Parameters[0]
				let attr = p.ParameterType.GetCustomAttribute<SentOnAttribute>(true)
				where attr != null
				select new { Action = a, SentOn = attr }).ToList();

			if (!webhookHandlers.Any())
				return false; // none of our ambiguous candidates are specific webhook handlers

			if (ReadWebhookPayload(context, out var verb, out var route))
			{
				var matchingHandlers = webhookHandlers.Where(h => h.SentOn.Verb.Equals(verb, StringComparison.OrdinalIgnoreCase) && h.SentOn.Route == route).ToList();
				if (matchingHandlers.Count == 1)
				{
					// found exactly 1 matching webhook handler. woo!
					result = matchingHandlers[0].Action;
					return true;
				}
				else if (matchingHandlers.Count > 1)
				{
					return false; // more than 1 matching webhook handler
				}
			}

			// didn't find any matching webhook handlers, but if we have exactly 1 match that isn't a specific webhook handler, use that.
			var nonHandlers = candidates.Except(webhookHandlers.Select(h => h.Action)).ToList();
			if (nonHandlers.Count == 1)
			{
				result = nonHandlers[0];
				return true;
			}

			return false;
		}

		private bool ReadWebhookPayload(RouteContext context, out string verb, out string route)
		{
			verb = null;
			route = null;

			// makes the request stream seekable so we can rewind it when we're done
			context.HttpContext.Request.EnableRewind();

			try
			{
				// don't wrap these in "using"s or they'll close the Request stream,
				// which will cause the payload object to be null in the action.
				var sr = new StreamReader(context.HttpContext.Request.Body);
				var jr = new JsonTextReader(sr);
				while (jr.Read())
				{
					if (jr.TokenType == JsonToken.PropertyName)
					{
						if (jr.Value as string == "Route")
						{
							jr.Read();
							route = jr.Value as string ?? route;
						}
						else if (jr.Value as string == "Verb")
						{
							jr.Read();
							verb = jr.Value as string ?? verb;
						}

						if (route != null && verb != null)
							return true;
					}
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				// rewind the request stream
				context.HttpContext.Request.Body.Position = 0;
			}
		}
	}
}
