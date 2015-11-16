using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DayBar.Contract.Strip;
using DayBar.Entity.Strip;

namespace Daybar.Core.Model.Strip
{
    public class Strip : IStrip
    {
        private readonly IStripWriter _writer;
        private int _range;
        public List<StripElement> Elements { get; } = new List<StripElement>();

        public Strip(StripSize size, IStripWriter writer)
        {
            _writer = writer;

            if (size == StripSize.Size_144)
            {
                _range = 144;
            }
        }

        public bool HasTag(string tag)
        {
            return Elements.Count(_ => tag == _.Tag) > 0;
        }

        public void CheckTags(List<string> tags)
        {
            var elements = Elements.Where(_ => _.Tag != null && !tags.Contains(_.Tag)).ToList();

            foreach (var element in elements)
            {
                Remove(element);
            }
        } 

        public void Remove(StripElement elementToRemove)
        {
            Debug.WriteLine($"****Removing:" + elementToRemove.Start);
            Elements.Remove(elementToRemove);
            elementToRemove.Type = StripElementType.SolidRange;
            elementToRemove.Red = 0;
            elementToRemove.Green = 0;
            elementToRemove.Blue = 0;

            if (elementToRemove.Length == 0)
            {
                elementToRemove.Type = StripElementType.SolidDot;
            }

            //clear this bit
            _draw(elementToRemove);
        
            foreach (var element in Elements)
            {
                if (element == elementToRemove)
                {
                    continue;
                }
                if (element.Start >= elementToRemove.Start &&
                    element.Start + element.Length <= elementToRemove.Start + elementToRemove.Length)
                {
                    //redraw this item as it intersected the removed item
                    _draw(element);
                }
            }
        }

        void _add(StripElement element)
        {
            Elements.Add(element);
            _draw(element);
        }

        void _draw(StripElement element)
        {
            string command = "";
            switch (element.Type)
            {
                case StripElementType.FlashDot:
                    command = $"d {element.Start} {element.Red} {element.Green} {element.Blue}";
                    break;

                case StripElementType.FlashRange:
                    command = $"e {element.Start} {element.Red} {element.Green} {element.Blue} {element.Length}";
                    break;

                case StripElementType.SolidDot:
                    command = $"a {element.Start} {element.Red} {element.Green} {element.Blue}";
                    break;

                case StripElementType.SolidRange:
                    command = $"b {element.Start} {element.Red} {element.Green} {element.Blue} {element.Length}";
                    break;
            }
            SendCommand(command);
        }

        public void SendCommand(string command)
        {
            _writer.Write(command);
        }

        public void Rainbow()
        {
            var command = "c";
            SendCommand(command);
        }

        public void SolidDot(int dot, int red, int green, int blue, string tag = null)
        {
            var element = new StripElement
            {
                Start = dot,
                Red = red,
                Green = green,
                Blue = blue,
                Type = StripElementType.SolidDot,
                Tag = tag
            };
            _add(element);
        }

        public void FlashDot(int dot, int red, int green, int blue, string tag = null)
        {

            //find the old flash if there was one
            
            var old = Elements.FirstOrDefault(_ => _.Type == StripElementType.FlashDot);

            if (old != null)
            {
                Remove(old);
            }

            var element = new StripElement
            {
                Start = dot,
                Red = red,
                Green = green,
                Blue = blue,
                Type = StripElementType.FlashDot,
                Tag = tag
            };

            Debug.WriteLine($"****Flashing:" + element.Start);

            _add(element);
        }

        public void FlashRange(int start, int length, int red, int green, int blue, string tag = null)
        {
            var element = new StripElement
            {
                Start = start,
                Length = length,
                Red = red,
                Green = green,
                Blue = blue,
                Type = StripElementType.FlashRange,
                Tag = tag
            };
            _add(element);
        }

        public void SetRange(int start, int length, int red, int green, int blue, string tag = null)
        {
            var element = new StripElement
            {
                Start = start,
                Length = length,
                Red = red,
                Green = green,
                Blue = blue,
                Type = StripElementType.SolidRange,
                Tag = tag
            };
            _add(element);
        }

        public void Clear()
        {
            Elements.Clear();
            SendCommand("f");
        }
    }
}
