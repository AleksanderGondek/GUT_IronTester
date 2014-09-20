using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IronTester.Common.Commands;
using IronTester.Common.Messages.Builds;
using IronTester.Common.Messages.Initialization;
using IronTester.Common.Messages.Tests;
using IronTester.Common.Messages.Validation;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.ManualMessagesPusher
{
    public class ManualMessagesPusher : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }
        
        public void Start()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Manual Message Pusher ready.");
            Console.WriteLine("To exit, Ctrl + C");
            Console.ResetColor();

            Console.WriteLine("Press anything to start.");

            while (Console.ReadLine() != null)
            {
                for (var i = 0; i < 10; i++)
                {
                    var guid = Guid.NewGuid();

                    Bus.Send("IronTester.Server",
                        Bus.CreateInstance<PleaseDoTests>(y =>
                                                          {
                                                              y.RequestId = guid;
                                                              y.SourceCodeLocation =
                                                                  GetRandomSourcePath();
                                                              y.TestsRequested =
                                                                  new List<string>
                                                                  {
                                                                      GetRandomTest(),
                                                                      GetRandomTest(),
                                                                      GetRandomTest()
                                                                  };
                                                          }));
                    Thread.Sleep(2000);
                }

                //Parallel.For(0, 100, x =>
                //                     {
                //                         var guid = Guid.NewGuid();

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<PleaseDoTests>(y =>
                //                                                               {
                //                                                                   y.RequestId = guid;
                //                                                                   y.SourceCodeLocation =
                //                                                                       RandomString(10);
                //                                                                   y.TestsRequested =
                //                                                                       new List<string>
                //                                                                       {
                //                                                                           RandomString
                //                                                                               (3),
                //                                                                           RandomString
                //                                                                               (3),
                //                                                                           RandomString
                //                                                                               (3)
                //                                                                       };
                //                                                               }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IValidationRequestConfirmation>(y =>
                //                                                                                {
                //                                                                                    y.RequestId = guid;
                //                                                                                    y.WillValidate =
                //                                                                                        true;
                //                                                                                    y.DenialReson = null;
                //                                                                                }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IValidationStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 0;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IValidationStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 25;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IValidationStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 50;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IValidationStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 75;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IValidationStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 100;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IValidationFinished>(y =>
                //                                                                     {
                //                                                                         y.RequestId = guid;
                //                                                                         y.ValidationSuccessful = true;
                //                                                                         y.ValidationFailReason = null;
                //                                                                     }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IInitializeRequestConfirmation>(y =>
                //                                                                                {
                //                                                                                    y.RequestId = guid;
                //                                                                                    y.WillInitialize =
                //                                                                                        true;
                //                                                                                    y.DenialReason =
                //                                                                                        null;
                //                                                                                }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IInitializeStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 0;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IInitializeStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 25;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IInitializeStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 50;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IInitializeStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 75;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IInitializeStatus>(y =>
                //                                                                   {
                //                                                                       y.RequestId = guid;
                //                                                                       y.Progress = 100;
                //                                                                   }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IInitializeFinished>(y =>
                //                                                                     {
                //                                                                         y.RequestId = guid;
                //                                                                         y.InitializationSuccessful =
                //                                                                             true;
                //                                                                         y.InitializationFailReason =
                //                                                                             null;
                //                                                                         y.PathToInitializedFiles =
                //                                                                             RandomString(20);
                //                                                                     }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IBuildsRequestConfirmation>(y =>
                //                                                                            {
                //                                                                                y.RequestId = guid;
                //                                                                                y.WillBuild = true;
                //                                                                                y.DenialReason = null;
                //                                                                            }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IBuildsStatus>(y =>
                //                                                               {
                //                                                                   y.RequestId = guid;
                //                                                                   y.Progress = 0;
                //                                                               }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IBuildsStatus>(y =>
                //                                                               {
                //                                                                   y.RequestId = guid;
                //                                                                   y.Progress = 25;
                //                                                               }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IBuildsStatus>(y =>
                //                                                               {
                //                                                                   y.RequestId = guid;
                //                                                                   y.Progress = 50;
                //                                                               }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IBuildsStatus>(y =>
                //                                                               {
                //                                                                   y.RequestId = guid;
                //                                                                   y.Progress = 75;
                //                                                               }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IBuildsStatus>(y =>
                //                                                               {
                //                                                                   y.RequestId = guid;
                //                                                                   y.Progress = 100;
                //                                                               }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<IBuildsFinished>(y =>
                //                                                                 {
                //                                                                     y.RequestId = guid;
                //                                                                     y.BuildsSuccessful = true;
                //                                                                     y.BuildsFailReason = null;
                //                                                                     y.PathToBuildsArtifacts =
                //                                                                         RandomString(20);
                //                                                                 }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<ITestsRequestConfirmation>(y =>
                //                                                                           {
                //                                                                               y.RequestId = guid;
                //                                                                               y.WillTest = true;
                //                                                                               y.DenialReason = null;
                //                                                                           }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<ITestsStatus>(y =>
                //                                                              {
                //                                                                  y.RequestId = guid;
                //                                                                  y.Progress = 0;
                //                                                              }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<ITestsStatus>(y =>
                //                                                              {
                //                                                                  y.RequestId = guid;
                //                                                                  y.Progress = 25;
                //                                                              }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<ITestsStatus>(y =>
                //                                                              {
                //                                                                  y.RequestId = guid;
                //                                                                  y.Progress = 50;
                //                                                              }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<ITestsStatus>(y =>
                //                                                              {
                //                                                                  y.RequestId = guid;
                //                                                                  y.Progress = 75;
                //                                                              }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<ITestsStatus>(y =>
                //                                                              {
                //                                                                  y.RequestId = guid;
                //                                                                  y.Progress = 100;
                //                                                              }));

                //                         Thread.Sleep(4000);

                //                         Bus.Send("IronTester.Server",
                //                             Bus.CreateInstance<ITestsFinished>(y =>
                //                                                                {
                //                                                                    y.RequestId = guid;
                //                                                                    y.TestsSuccessful = true;
                //                                                                    y.TestsFailReason = null;
                //                                                                    y.TestsArtifactsLocation =
                //                                                                        RandomString(20);
                //                                                                }));

                //                         Thread.Sleep(4000);
                //                     });
            }
        }

        public void Stop()
        {
            //Nothing
        }


        public static string RandomString(int size)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string GetRandomTest()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var index = random.Next(ValidationData.ValidTests.Count);
            return ValidationData.ValidTests.ElementAt(index);
        }

        public static string GetRandomSourcePath()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var index = random.Next(ValidationData.ValidInitializationPaths.Count);
            return ValidationData.ValidInitializationPaths.ElementAt(index);
        }
    }
}
