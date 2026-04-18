using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.GUI.Objects;

public record TextDefinition(string Text, string Color, int Size, bool Bold = false, TextAnchor TextAnchor = TextAnchor.UpperLeft);

