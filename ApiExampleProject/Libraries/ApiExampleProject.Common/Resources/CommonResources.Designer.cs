﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiExampleProject.Common.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CommonResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CommonResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ApiExampleProject.Common.Resources.CommonResources", typeof(CommonResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Content: .
        /// </summary>
        internal static string ContentMessage {
            get {
                return ResourceManager.GetString("ContentMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duration: {0}.
        /// </summary>
        internal static string DurationMessage {
            get {
                return ResourceManager.GetString("DurationMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HttpClient request for {0} {1} {2}/{3}.
        /// </summary>
        internal static string HttpClientRequestMessage {
            get {
                return ResourceManager.GetString("HttpClientRequestMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HttpClient response for {0}/{1} {2} {3}.
        /// </summary>
        internal static string HttpClientResponseMessage {
            get {
                return ResourceManager.GetString("HttpClientResponseMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ==============================.
        /// </summary>
        internal static string LineSeparator {
            get {
                return ResourceManager.GetString("LineSeparator", resourceCulture);
            }
        }
    }
}
