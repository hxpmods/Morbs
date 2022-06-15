using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Morbs.Utility
{
    public class ReplaceAnimationComp : MonoBehaviour
    {

        private SpriteRenderer sr;
        private Image i;

        public static Dictionary<string, List<Sprite>> sprite_sheets = new Dictionary<string, List<Sprite>>();

        public string animation_set = "";

        private int frameCounter = 0;

        public string lastRecordedSprite = "";

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();

            i = GetComponent<Image>();
        }

        void IncrementFrameCounter()
        {
            frameCounter = (frameCounter + 1) % sprite_sheets[animation_set].Count;
        }

        void LateUpdate()
        {
            if (animation_set != "")
            {

                if (sprite_sheets.ContainsKey(animation_set))
                {
                    var sprite_sheet = sprite_sheets[animation_set];

                    if (sr)
                    {
                        var thisFrame = this.sr.sprite.name;

                        if (lastRecordedSprite != "")
                        {
                            if (thisFrame != lastRecordedSprite)
                            {
                                IncrementFrameCounter();
                            }

                        }
                        lastRecordedSprite = thisFrame;

                        sr.sprite = sprite_sheet[frameCounter];
                    }

                    if (i)
                    {
                        var thisFrame = this.i.sprite.name;

                        if (lastRecordedSprite != "")
                        {
                            if (thisFrame != lastRecordedSprite)
                            {
                                IncrementFrameCounter();
                            }

                        }

                        lastRecordedSprite = thisFrame;

                        i.overrideSprite = sprite_sheet[frameCounter];
                    }
                }
            }
        }
    }
}
