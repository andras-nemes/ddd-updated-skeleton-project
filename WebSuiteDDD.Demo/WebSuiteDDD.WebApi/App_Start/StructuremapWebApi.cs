// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuremapWebApi.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.Web.Http;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using StructureMap;
using WebSuiteDDD.WebApi.DependencyResolution;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(WebSuiteDDD.WebApi.App_Start.StructuremapWebApi), "Start")]

namespace WebSuiteDDD.WebApi.App_Start
{
	public static class StructuremapWebApi
	{
		public static void Start()
		{
			IContainer container = IoC.Initialize();
			container.AssertConfigurationIsValid();
			Debug.WriteLine(container.WhatDoIHave());  
			GlobalConfiguration.Configuration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);
		}
	}
}