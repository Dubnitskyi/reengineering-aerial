using System;

namespace Aerial
{
    public static class AssemblyVersion
    {
        /// <summary>
        /// Gets the executing assembly.
        /// </summary>
        /// <value>The executing assembly.</value>
        public static System.Reflection.Assembly ExecutingAssembly
        {
            get { return executingAssembly ?? (executingAssembly = System.Reflection.Assembly.GetExecutingAssembly()); }
        }

        private static System.Reflection.Assembly executingAssembly;

        /// <summary>
        /// Gets the executing assembly version.
        /// </summary>
        /// <value>The executing assembly version.</value>
        public static System.Version ExecutingAssemblyVersion
        {
            get { return executingAssemblyVersion ?? (executingAssemblyVersion = ExecutingAssembly.GetName().Version); }
        }
        private static System.Version executingAssemblyVersion;

        /// <summary>
        /// Gets the compile date of the currently executing assembly.
        /// </summary>
        /// <value>The compile date.</value>
        public static System.DateTime CompileDate
        {
            get
            {
                if (!compileDate.HasValue)
                    compileDate = System.IO.File.GetLastWriteTime(ExecutingAssembly.Location);
                return compileDate.Value;
            }
        }
        private static System.DateTime? compileDate;
    }
}
