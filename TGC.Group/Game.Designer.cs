﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TGC.Group {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Game : global::System.Configuration.ApplicationSettingsBase {
        
        private static Game defaultInstance = ((Game)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Game())));
        
        public static Game Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Tomas De Pietro")]
        public string Category {
            get {
                return ((string)(this["Category"]));
            }
            set {
                this["Category"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Madness")]
        public string Name {
            get {
                return ((string)(this["Name"]));
            }
            set {
                this["Name"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Tron - Grid Game")]
        public string Description {
            get {
                return ((string)(this["Description"]));
            }
            set {
                this["Description"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("..\\..\\Shaders\\")]
        public string ShadersDirectory {
            get {
                return ((string)(this["ShadersDirectory"]));
            }
            set {
                this["ShadersDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("..\\..\\Media\\")]
        public string MediaDirectory {
            get {
                return ((string)(this["MediaDirectory"]));
            }
            set {
                this["MediaDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("cajaMadera4.jpg")]
        public string TexturaCaja {
            get {
                return ((string)(this["TexturaCaja"]));
            }
            set {
                this["TexturaCaja"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MeshCreator\\\\Meshes\\\\Vehiculos\\\\Auto\\\\Auto-TgcScene.xml")]
        public string pathAuto {
            get {
                return ((string)(this["pathAuto"]));
            }
            set {
                this["pathAuto"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SkyBoxTron\\\\")]
        public string PathSkybox {
            get {
                return ((string)(this["PathSkybox"]));
            }
            set {
                this["PathSkybox"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MeshCreator\\\\Meshes\\\\Vehiculos\\\\Moto\\\\Moto-TgcScene.xml")]
        public string pathMoto {
            get {
                return ((string)(this["pathMoto"]));
            }
            set {
                this["pathMoto"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CajaMetalFuturistica\\\\CajaMetalFuturistica-TgcScene.xml")]
        public string pathCajaMetalica {
            get {
                return ((string)(this["pathCajaMetalica"]));
            }
            set {
                this["pathCajaMetalica"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("musica.mp3")]
        public string pathMusica {
            get {
                return ((string)(this["pathMusica"]));
            }
            set {
                this["pathMusica"] = value;
            }
        }
    }
}
