﻿using UnityEngine;
using System.Collections;

namespace JBooth.VertexPainterPro
{
   public class VertexPainterCustomBrush : ScriptableObject
   {

      public enum Channels
      {
         Colors = 1,
         UV0    = 2,
         UV1    = 4,
         UV2    = 8,
         UV3    = 16,
         Normals = 32,
         Positions = 64
      }
      // preview color for the brush
      public virtual Color GetPreviewColor()
      {
         return Color.yellow;
      }

      // return a bitmask of channels in use, so Channels.Colors | Channels.UV0 if you affect those channels with your brush..
      public virtual Channels GetChannels()
      {
         Debug.LogError("GetChannels not implimented in custom brush!");
         return 0;
      }
         
      // return a delegate to modify the vert by lerping it towards a given value
      public virtual VertexPainterWindow.Lerper GetLerper()
      {
         Debug.LogError("Lerper not implimented in custom brush!");
         return null;
      }
      public virtual object GetBrushObject()
      {
         Debug.LogError("GetBrushObject not implimented in custom brush");
         return null;
      }

      // function in case you'd like to draw some custom gui for your brush
      public virtual void DrawGUI()
      {
      }
   }
}
