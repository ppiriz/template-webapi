FAQ
===
When generating from autorest, I get the error

		Code generation failed.
		AutoRest.exe -CSharp -ModelsName ...
		FATAL: Error parsing swagger file. Error converting value "modelbinding" to type 'AutoRest.Swagger.Model.ParameterLocation'. Path 'get.parameters[0].in', line x, position y. 

Solution
		You must have all of your API action parameters (GET/POST/etc) decorated with attributes specifing the source of the param. Eg [FromBody] / [FromUri]
		see: https://github.com/Azure/autorest/issues/1639






I get the exception

		System.TypeLoadException
		Inheritance security rules violated by type: 'System.Net.Http.WebRequestHandler'. Derived types must either match the security accessibility of the base type or be less accessible.

Solution
Update the web/app.config to the below. NOTE the newVersion. the 4.1.1.0 version has an issue.

	  <dependentAssembly>
        <assemblyIdentity name="System.Net.Http" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.1.1.0" newVersion="4.0.0.0" />
      </dependentAssembly>