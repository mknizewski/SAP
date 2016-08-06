using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace SAP.BOL.LogicClasses
{
    /// <summary>
    /// Autor:  Mateusz Kniżewski
    /// Data:   07-08-2016
    /// Opis:   Klasa umożliwjająca uruchamianie programów w piaskownicy
    /// </summary>
    public class Sandbox : MarshalByRefObject
    {
        private Evidence _evidence { get; set; }
        private PermissionSet _internetPermissionSet { get; set; }
        private Sandbox _internetSandBox { get; set; }

        private const string _path = @"D:\SAP\SAP\SAP.Workers\bin\Debug";
        private const string _untrustedAssembly = "SAP.Workers";
        private const string _untrustedClass = "SAP.Workers.Program";
        private const string _entryPoint = "NoElo";

        public Sandbox()
        {
            _evidence = new Evidence();
            _evidence.AddHostEvidence(new Zone(SecurityZone.Internet));

           _internetPermissionSet = SecurityManager.GetStandardSandbox(_evidence);
        }

        public static Sandbox Create()
        {
            var sandbox = new Sandbox();

            return sandbox;
        }
        
        public void ExecuteUntrusedCode(object[] parameters)
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup();

            appDomainSetup.ApplicationBase = Path.GetFullPath(_path);

            StrongName fullTrustAssembly = typeof(Sandbox).Assembly.Evidence.GetHostEvidence<StrongName>();
            AppDomain appDomain = AppDomain.CreateDomain("Sandobx", _evidence, appDomainSetup, _internetPermissionSet, fullTrustAssembly);

            ObjectHandle handle = Activator.CreateInstanceFrom(
                appDomain,
                typeof(Sandbox).Assembly.ManifestModule.FullyQualifiedName,
                typeof(Sandbox).FullName);
            //appDomain.ExecuteAssembly(@"%windir%\system32\notepad.exe");
            //Sandbox newDomainInstance = (Sandbox)handle.Unwrap();
            //newDomainInstance.Execute(null);
        }
    }
}