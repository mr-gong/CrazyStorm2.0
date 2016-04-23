﻿/*
 * The MIT License (MIT)
 * Copyright (c) StarX 2015 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using CrazyStorm.Core;

namespace CrazyStorm
{
    class BindComponentCommand : Command
    {
        public override void Redo(CommandStack stack)
        {
            base.Redo(stack);
            var bindingLines = Parameter[0] as IList<Line>;
            var boundComponents = new List<Component>();
            var selectedComponent = Parameter[1] as Component;
            foreach (var line in bindingLines)
            {
                var component = line.DataContext as Component;
                boundComponents.Add(component.BindingTarget);
                if (component != selectedComponent)
                    component.BindingTarget = selectedComponent;
            }
            History[0] = boundComponents;
        }
        public override void Undo(CommandStack stack)
        {
            base.Undo(stack);
            var bindingLines = Parameter[0] as IList<Line>;
            var boundComponents = History[0] as IList<Component>;
            foreach (var line in bindingLines)
            {
                var component = line.DataContext as Component;
                component.BindingTarget = boundComponents.First();
                boundComponents.RemoveAt(0);
            }
        }
    }
}
