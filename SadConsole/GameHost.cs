﻿using System;
using System.Collections.Generic;
using System.Text;
using SadRogue.Primitives;
using SadConsole.Renderers;
using System.IO;

namespace SadConsole
{
    public abstract class GameHost : IDisposable
    {
        /// <summary>
        /// Instance of the game host.
        /// </summary>
        public static GameHost Instance { get; set; }

        protected static bool LoadingEmbeddedFont = false;
        protected internal static string SerializerPathHint { get; set; }

        /// <summary>
        /// Raised when the game draws a frame.
        /// </summary>
        public event EventHandler<GameHost> FrameDraw;

        /// <summary>
        /// Raised when the game updates prior to drawing a frame.
        /// </summary>
        public event EventHandler<GameHost> FrameUpdate;

        /// <summary>
        /// A callback to run before the <see cref="Run"/> method is called;
        /// </summary>
        public Action OnStart;

        /// <summary>
        /// A callback to run after the <see cref="Run"/> method is called;
        /// </summary>
        public Action OnEnd;

        /// <summary>
        /// Collection of fonts. Used mainly by the deserialization system.
        /// </summary>
        public Dictionary<string, Font> Fonts { get; } = new Dictionary<string, Font>();

        /// <summary>
        /// The default font for any type that does not provide a font.
        /// </summary>
        public Font DefaultFont { get; set; }

        /// <summary>
        /// The default font to use with <see cref="DefaultFont"/>.
        /// </summary>
        public Font.Sizes DefaultFontSize { get; set; }

        /// <summary>
        /// Draw calls registered for the next drawing frame.
        /// </summary>
        public Queue<DrawCalls.IDrawCall> DrawCalls { get; } = new Queue<DrawCalls.IDrawCall>();

        public Point WindowSize { get; }

        public int ScreenCellsX { get; protected set; }
        public int ScreenCellsY { get; protected set; }

        /// <summary>
        /// Raises the <see cref="FrameDraw"/> event.
        /// </summary>
        protected virtual void OnFrameDraw() =>
            FrameDraw?.Invoke(this, this);

        /// <summary>
        /// Raises the <see cref="FrameUpdate"/> event.
        /// </summary>
        protected virtual void OnFrameUpdate() =>
            FrameUpdate?.Invoke(this, this);

        public abstract void Run();

        public abstract ITexture GetTexture(string resourcePath);

        public abstract ITexture GetTexture(Stream textureStream);

        public abstract IRenderer GetDefaultRenderer();

        /// <summary>
        /// Loads a font from a file and adds it to the <see cref="Fonts"/> collection.
        /// </summary>
        /// <param name="font">The font file to load.</param>
        /// <returns>A master font that you can generate a usable font from.</returns>
        public Font LoadFont(string font)
        {
            //if (!File.Exists(font))
            //{
            //    font = Path.Combine(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(font)), "fonts"), Path.GetFileName(font));
            //    if (!File.Exists(font))
            //        throw new Exception($"Font does not exist: {font}");
            //}                    

            //FontPathHint = Path.GetDirectoryName(Path.GetFullPath(font));
            try
            {
                Font masterFont = SadConsole.Serializer.Load<Font>(font, false);

                if (Fonts.ContainsKey(masterFont.Name))
                {
                    Fonts.Remove(masterFont.Name);
                }

                Fonts.Add(masterFont.Name, masterFont);
                return masterFont;
            }
            catch (System.Runtime.Serialization.SerializationException)
            {

                throw;
            }

        }

        /// <summary>
        /// Opens a file stream with the specified mode and access.
        /// </summary>
        /// <param name="file">The file to open.</param>
        /// <param name="mode">The mode for opening. Defaults to <see cref="FileMode.Open"/>.</param>
        /// <param name="access">The type of access for the stream. Defaults to <see cref="FileAccess.Read"/>.</param>
        /// <returns>The stream object.</returns>
        public virtual Stream OpenStream(string file, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read) =>
            File.Open(file, mode, access);

        /// <summary>
        /// Loads the <c>IBM.font</c> built into the binary.
        /// </summary>
        protected void LoadEmbeddedFont()
        {
            //var auxList = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
//#if WINDOWS_UWP || WINDOWS_UAP
            System.Reflection.Assembly assembly = typeof(SadConsole.Font).Assembly;
//#else
            //var assembly = System.Reflection.Assembly.GetExecutingAssembly();
//#endif
            string resourceNameFont;
            string resourceNameImage;

            if (Settings.UseDefaultExtendedFont)
            {
                resourceNameFont = "SadConsole.Resources.IBM_ext.font";
                resourceNameImage = "SadConsole.Resources.IBM8x16_NoPadding_extended.png";
            }
            else
            {
                resourceNameFont = "SadConsole.Resources.IBM.font";
                resourceNameImage = "SadConsole.Resources.IBM8x16.png";
            }


            using (Stream stream = assembly.GetManifestResourceStream(resourceNameFont))
            using (StreamReader sr = new StreamReader(stream))
            {
                LoadingEmbeddedFont = true;
                SerializerPathHint = "";
                var masterFont = (Font)Newtonsoft.Json.JsonConvert.DeserializeObject(
                    sr.ReadToEnd(),
                    typeof(Font),
                    new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
                    });

                using (Stream fontStream = assembly.GetManifestResourceStream(resourceNameImage))
                    masterFont.Image = GetTexture(fontStream);

                masterFont.ConfigureRects();
                Instance.Fonts.Add(masterFont.Name, masterFont);
                Instance.DefaultFont = masterFont;

                LoadingEmbeddedFont = false;
            }
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var font in Fonts.Values)
                    {
                        font.Image.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Game()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
