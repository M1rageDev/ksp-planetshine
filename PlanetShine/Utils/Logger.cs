#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

#endregion

namespace PlanetShine.Utils {
	[KSPAddon(KSPAddon.Startup.Instantly, false)]
	public class Logger : MonoBehaviour {
		#region Constants

		private static readonly string FileName;
		private static readonly AssemblyName AssemblyName;

		#endregion

		#region Fields

		private static readonly List<string[]> Messages = new List<string[]>();

		#endregion

		#region Initialisation

		static Logger() {
			AssemblyName = Assembly.GetExecutingAssembly().GetName();
			FileName = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "log");
			File.Delete(FileName);

			lock (Messages) {
				Messages.Add(new[] { "Executing: " + AssemblyName.Name + " - " + AssemblyName.Version });
				Messages.Add(new[] { "Assembly: " + Assembly.GetExecutingAssembly().Location });
			}
			Blank();
		}

		private void Awake() {
			DontDestroyOnLoad(this);
		}

		#endregion

		#region Printing

		public static void Blank() {
			lock (Messages) {
				Messages.Add(new string[] { });
			}
		}

		public static void Log(object obj) {
			lock (Messages) {
				try {
					if (obj is IEnumerable) {
						Messages.Add(new[] { "Log " + DateTime.Now.TimeOfDay, obj.ToString() });
						foreach (var o in obj as IEnumerable) {
							Messages.Add(new[] { "\t", o.ToString() });
						}
					} else {
						Messages.Add(new[] { "Log " + DateTime.Now.TimeOfDay, obj.ToString() });
					}
				}
				catch (Exception ex) {
					Exception(ex);
				}
			}
		}

		public static void Log(string name, object obj) {
			lock (Messages) {
				try {
					if (obj is IEnumerable) {
						Messages.Add(new[] { "Log " + DateTime.Now.TimeOfDay, name });
						foreach (var o in obj as IEnumerable) {
							Messages.Add(new[] { "\t", o.ToString() });
						}
					} else {
						Messages.Add(new[] { "Log " + DateTime.Now.TimeOfDay, obj.ToString() });
					}
				}
				catch (Exception ex) {
					Exception(ex);
				}
			}
		}

		public static void Log(string message) {
			lock (Messages) {
				Messages.Add(new[] { "Log " + DateTime.Now.TimeOfDay, message });
			}
		}

		[System.Diagnostics.Conditional("DEBUG")]
		[System.Diagnostics.DebuggerStepThrough]
		public static void Debug(string message) {
			lock (Messages) {
				Messages.Add(new[] { "Debug " + DateTime.Now.TimeOfDay, message });
			}
		}

		public static void Warning(string message) {
			lock (Messages) {
				Messages.Add(new[] { "Warning " + DateTime.Now.TimeOfDay, message });
			}
		}

		public static void Error(string message) {
			lock (Messages) {
				Messages.Add(new[] { "Error " + DateTime.Now.TimeOfDay, message });
			}
		}

		public static void Exception(Exception ex) {
			lock (Messages) {
				Messages.Add(new[] { "Exception " + DateTime.Now.TimeOfDay, ex.Message });
				Messages.Add(new[] { string.Empty, ex.StackTrace });
				Blank();
			}
		}

		public static void Exception(Exception ex, string location) {
			lock (Messages) {
				Messages.Add(new[] { "Exception " + DateTime.Now.TimeOfDay, location + " // " + ex.Message });
				Messages.Add(new[] { string.Empty, ex.StackTrace });
				Blank();
			}
		}

		#endregion

		#region Flushing

		public static void Flush() {
			lock (Messages) {
				if (Messages.Count > 0) {
					using (var file = File.AppendText(FileName)) {
						foreach (var message in Messages) {
							file.WriteLine(message.Length > 0 ? message.Length > 1 ? "[" + message[0] + "]: " + message[1] : message[0] : string.Empty);
							if (message.Length > 0) {
								print(message.Length > 1 ? AssemblyName.Name + " -> " + message[1] : AssemblyName.Name + " -> " + message[0]);
							}
						}
					}
					Messages.Clear();
				}
			}
		}

		private void LateUpdate() {
			Flush();
		}

		#endregion

		#region Destruction

		private void OnDestroy() {
			Flush();
		}

		~Logger() {
			Flush();
		}

		#endregion
	}
}
