// Decompiled with JetBrains decompiler
// Type: Sitecore.DataExchange.Providers.Salesforce.Endpoints.ProtectedDataHelper
// Assembly: Sitecore.DataExchange.Providers.Salesforce, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3235E6D4-4DB2-46F6-BF8E-FA4BFDDB59BE
// Assembly location: C:\inetpub\wwwroot\xp102sc.dev.local\bin\Sitecore.DataExchange.Providers.Salesforce.dll

using System.Security.Cryptography;
using System.Text;

namespace Sitecore.DataExchange.Providers.Salesforce.Endpoints
{
  public static class ProtectedDataHelper
  {
    private static byte[] _entropy;

    internal static byte[] ProtectData(string plaintext) => ProtectedData.Protect(ProtectedDataHelper.ConvertToBytes(plaintext), ProtectedDataHelper.CreateEntropy(), DataProtectionScope.CurrentUser);

    internal static string UnprotectData(byte[] bytesData) => ProtectedDataHelper.ConvertToString(ProtectedData.Unprotect(bytesData, ProtectedDataHelper.CreateEntropy(), DataProtectionScope.CurrentUser));

    private static byte[] CreateEntropy()
    {
      if (ProtectedDataHelper._entropy != null)
        return ProtectedDataHelper._entropy;
      byte[] data = new byte[20];
      using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
        cryptoServiceProvider.GetBytes(data);
      ProtectedDataHelper._entropy = data;
      return data;
    }

    private static byte[] ConvertToBytes(string plainText) => Encoding.ASCII.GetBytes(plainText);

    private static string ConvertToString(byte[] bytes) => Encoding.ASCII.GetString(bytes);
  }
}
