using System;
using System.Diagnostics;
using System.IO;
using ApiExampleProject.IntegrationTests.Configuration;

namespace ApiExampleProject.IntegrationTests.TestFixtures
{
    public abstract class BaseTestFixture
        : IDisposable
    {
        private readonly Process funcHostProcess;

        public BaseTestFixture(string functionAppPath, int portNumber)
        {
            _ = functionAppPath ?? throw new ArgumentNullException(nameof(functionAppPath));

            var functionHostPath = ConfigurationHelper.Settings.FunctionHostPath.Replace('|', Path.DirectorySeparatorChar);
            var functionAppFolder = Path.GetRelativePath(Directory.GetCurrentDirectory(), functionAppPath.Replace('|', Path.DirectorySeparatorChar));

            funcHostProcess = new Process()
            {
                StartInfo =
                {
                    FileName = functionHostPath,
                    Arguments = $"start -p {portNumber}",
                    WorkingDirectory = functionAppFolder
                }
            };

            var success = funcHostProcess.Start();
            if (!success)
            {
                throw new InvalidOperationException(IntegrationTestResources.CouldNotStart_ErrorMessage);
            }

            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                funcHostProcess.Close();
                funcHostProcess.Dispose();
            }
        }
    }
}
