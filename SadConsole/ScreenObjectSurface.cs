﻿using System;
using System.Collections.Generic;
using System.Text;
using SadRogue.Primitives;

namespace SadConsole
{
    /// <summary>
    /// An object that renders a <see cref="CellSurface"/>.
    /// </summary>
    public class ScreenObjectSurface: ScreenObject
    {
        private Font _font;
        private Point _fontSize;
        private Renderers.IRenderer _renderer;
        private Color _tint;
        private bool _usePixelPositioning;

        /// <summary>
        /// The surface used by the screen object.
        /// </summary>
        public CellSurface Surface { get; protected set; }

        /// <summary>
        /// Indicates the <see cref="Surface"/> has changed and needs to be rendered.
        /// </summary>
        public bool IsDirty
        {
            get => Surface.IsDirty;
            set => Surface.IsDirty = value;
        }

        /// <summary>
        /// The renderer used to draw this surface.
        /// </summary>
        public Renderers.IRenderer Renderer
        {
            get => _renderer;
            set
            {
                _renderer = value;
                IsDirty = true;
            }
        }

        /// <summary>
        /// Font used with rendering.
        /// </summary>
        public Font Font
        {
            get => _font;
            set
            {
                if (_font == value) return;

                _font = value;
                FontSize = _font.GetFontSize(Font.Sizes.One);
                IsDirty = true;
            }
        }

        /// <summary>
        /// The size of the <see cref="Font"/> cells applied to the <see cref="Surface"/> when rendering.
        /// </summary>
        public Point FontSize
        {
            get => _fontSize;
            set
            {
                if (_fontSize == value) return;

                _fontSize = value;
                IsDirty = true;
            }
        }

        /// <summary>
        /// A tint used in rendering.
        /// </summary>
        public Color Tint
        {
            get => _tint;
            set
            {
                _tint = value;
                IsDirty = true;
            }
        }

        /// <summary>
        /// The area on the screen this surface occupies. In pixels.
        /// </summary>
        public Rectangle AbsoluteArea => new Rectangle(AbsolutePosition.X, AbsolutePosition.Y, Surface.Width * FontSize.X, Surface.Height * FontSize.Y);

        /// <summary>
        /// Treats the <see cref="ScreenObject.Position"/> of the console as if it is pixels and not cells.
        /// </summary>
        public bool UsePixelPositioning
        {
            get => _usePixelPositioning;
            set
            {
                _usePixelPositioning = value;
                SetAbsolutePosition();
            }
        }

        /// <summary>
        /// The width of the surface in pixels.
        /// </summary>
        public int WidthPixels => Surface.Width * FontSize.X;

        /// <summary>
        /// The height of the surface in pixels.
        /// </summary>
        public int HeightPixels => Surface.Height * FontSize.Y;

        /// <inheritdoc />
        protected override void SetAbsolutePosition()
        {
            if (UsePixelPositioning)
                base.SetAbsolutePosition();
            else
                AbsolutePosition = (FontSize * Position) + (Parent?.AbsolutePosition ?? new Point(0, 0));

            foreach (Console child in Children)
                child.SetAbsolutePosition();
        }

        ///  <inheritdoc/>
        public override void Draw()
        {
            if (!IsVisible) return;

            if (_renderer != null)
            {
                if (IsDirty)
                {
                    _renderer.Refresh(this);
                    IsDirty = false;
                }
                _renderer.Render(this);
                //Global.DrawCalls.Add(new DrawCalls.DrawCallScreenObject(this, CalculatedPosition, true));
            }

            base.Draw();
        }

        ///  <inheritdoc/>
        public override void Update()
        {
            if (!IsEnabled) return;

            //Effects.UpdateEffects(timeElapsed.TotalSeconds);

            base.Update();
        }

        /// <summary>
        /// Disposes <see cref="Renderer"/>.
        /// </summary>
        ~ScreenObjectSurface() =>
            _renderer?.Dispose();
    }
}
