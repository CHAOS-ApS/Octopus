using System;

namespace Geckon.Octopus.Plugins.Transcoding.Carbon
{
	public class CarbonVideoInformation
	{
		#region Fields

		#endregion
        #region Constructor

		public CarbonVideoInformation(uint width, uint height, uint audioBitRate, uint videoBitRate, ushort frameRate)
        {
            Width = width;
            Height = height;
            AudioBitRate = audioBitRate;
            VideoBitRate = videoBitRate;
            FrameRate = frameRate;
        }

        #endregion
        #region Methods

        public void ConstrainDimensionPreservingAspectRatio(uint maxWidth, uint maxHeight)
        {
            if (Width > maxWidth)
            {
                decimal xRatio =
                    (decimal)maxWidth /
                    Width;

                Width = (uint)Math.Round(Width * xRatio);
                Height = (uint)Math.Round(Height * xRatio);
            }

            if (Height > maxHeight)
            {
                decimal xRatio =
                    (decimal)maxHeight /
                    Height;

                Width = (uint)Math.Round(Width * xRatio);
                Height = (uint)Math.Round(Height * xRatio);
            }
        }

        #endregion
        #region Properties

		public uint Width { get; set; }

		public uint Height { get; set; }

		public uint AudioBitRate { get; set; }

		public uint VideoBitRate { get; set; }

		public ushort FrameRate { get; set; }

		#endregion
	}
}