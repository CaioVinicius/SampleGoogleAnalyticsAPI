using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Security.Cryptography.X509Certificates;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;

namespace PoCGoogleApis
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string[] scopes = new string[] {AnalyticsService.Scope.Analytics};//Query Google Analytics data

				const string arquivoChaveCaminho = @"C:\company-file-6ff1c2c38aef.p12";//Key path
                const string emailContaServico = "gapiuser@ecommerce-service-account.iam.gserviceaccount.com"; //inform Service account e-mail
                const string profileId = "123456";//profile id

                //load key file
                var certificado = new X509Certificate2(arquivoChaveCaminho, "notasecret", X509KeyStorageFlags.Exportable);
                var credential =
                    new ServiceAccountCredential(new ServiceAccountCredential.Initializer(emailContaServico)
                    {
                        Scopes = scopes
                    }.FromCertificate(certificado));

				//setting credentials to create a new service
                var service = new AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Exemplo Analytics API"
                });

				//GET request to Google Real-Time API
                DataResource.RealtimeResource.GetRequest request = service.Data.Realtime.Get(String.Format("ga:{0}", profileId), "rt:activeUsers");
                RealtimeData feed = request.Execute(); // retorna um objeto RealTimeData resource

				//each line returns a list of string with respective value
                //foreach (var item in feed.Rows)
                //{
                //    foreach (string col in item)
                //    {
                //        Console.Write(col+ " ");
                //    }
                //    Console.WriteLine("\r\n");
                //}
                Console.WriteLine("Users Online: " + feed.Rows[0][0]);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine("ERROR:" + ex.Message);
            }
            catch (GoogleApiException ex)
            {
                Console.WriteLine("ERROR:" + ex.Message);
            }
            catch (TokenResponseException ex)
            {
                Console.WriteLine("ERROR:"+ex.Message);
            }
            catch (AggregateException ex)
            {
                foreach (var item in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR:" + item.Message);
                }
            }
            Console.WriteLine("Digite qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}