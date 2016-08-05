﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hermes {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Hermes.ErrorMessages", typeof(ErrorMessages).Assembly);
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
        ///   Looks up a localized string similar to The scheduled date is invalid. Unable to create a DateTime for {0}..
        /// </summary>
        internal static string DateTimeFormatError {
            get {
                return ResourceManager.GetString("DateTimeFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An expression-field may not end with a * character..
        /// </summary>
        internal static string EndsWithMatchAllError {
            get {
                return ResourceManager.GetString("EndsWithMatchAllError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;You must first configure the environment settings before attempting to configure the Bus&quot;.
        /// </summary>
        internal static string EnvironmentNotConfigured {
            get {
                return ResourceManager.GetString("EnvironmentNotConfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provided cron expression must contain {0} fields..
        /// </summary>
        internal static string ExpressionFieldCountError {
            get {
                return ResourceManager.GetString("ExpressionFieldCountError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A value in the expression field is incorrectly formatted..
        /// </summary>
        internal static string ExpressionFieldFormatError {
            get {
                return ResourceManager.GetString("ExpressionFieldFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An invalid character was detected in the expression-field value..
        /// </summary>
        internal static string InvalidCharacterError {
            get {
                return ResourceManager.GetString("InvalidCharacterError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;A schedule could not be resolved. Check that the date is in fact reachable e.g. February 31 can never be scheduled.&quot;.
        /// </summary>
        internal static string NoScheduleFound {
            get {
                return ResourceManager.GetString("NoScheduleFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An expression-field may not only contain a / character..
        /// </summary>
        internal static string OnlyIncrementSeperatorError {
            get {
                return ResourceManager.GetString("OnlyIncrementSeperatorError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An expression-field may not only contain a - character..
        /// </summary>
        internal static string OnlyRangeSeperatorError {
            get {
                return ResourceManager.GetString("OnlyRangeSeperatorError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provided value of {0} is larger than the maximum allowed value of {1} for this field..
        /// </summary>
        internal static string OutOfRangeError {
            get {
                return ResourceManager.GetString("OutOfRangeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An expression-field may not end or start with with a / character..
        /// </summary>
        internal static string StartsOrEndsWithIncrementError {
            get {
                return ResourceManager.GetString("StartsOrEndsWithIncrementError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An expression-field may not end or start with with a - character..
        /// </summary>
        internal static string StartsOrEndsWithRangeError {
            get {
                return ResourceManager.GetString("StartsOrEndsWithRangeError", resourceCulture);
            }
        }
    }
}
