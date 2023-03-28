// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.ApiUsage.ApiUsageReader
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using Newtonsoft.Json.Linq;
using Salesforce.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.Salesforce.ApiUsage
{
  public class ApiUsageReader : IApiUsageReader
  {
    public ApiUsageDetails GetApiUsageDetails(AuthenticationClient client)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      int limit = 0;
      int remaining = 0;
      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.InstanceUrl + "/services/data/" + client.ApiVersion + "/limits");
      request.Headers.Add("Authorization", "Bearer " + client.AccessToken);
      request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      HttpClient queryClient = new HttpClient();
      HttpResponseMessage response = Task.Run<HttpResponseMessage>((Func<Task<HttpResponseMessage>>) (async () => await queryClient.SendAsync(request))).Result;
      foreach (KeyValuePair<string, JToken> keyValuePair in JObject.Parse(Task.Run<string>((Func<Task<string>>) (async () => await response.Content.ReadAsStringAsync())).Result))
      {
        if (keyValuePair.Key.Equals("DAILYAPIREQUESTS", StringComparison.InvariantCultureIgnoreCase))
        {
          limit = System.Convert.ToInt32(keyValuePair.Value[(object) "Max"].ToString());
          remaining = System.Convert.ToInt32(keyValuePair.Value[(object) "Remaining"].ToString());
          break;
        }
      }
      return new ApiUsageDetails(limit, remaining);
    }
  }
}
