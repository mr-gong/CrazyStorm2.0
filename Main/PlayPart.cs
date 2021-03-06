﻿/*
 * The MIT License (MIT)
 * Copyright (c) StarX 2016 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using CrazyStorm.Core;

namespace CrazyStorm
{
    public partial class Main
    {
        #region Private Methods
        void GeneratePlayFile()
        {
            if (string.IsNullOrWhiteSpace(Core.File.CurrentDirectory))
            {
                MessageBox.Show((string)FindResource("NeedSaveFirstStr"), (string)FindResource("TipTitleStr"),
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string genPath = Path.GetDirectoryName(filePath) + "\\" + fileName + ".bg";
            using (FileStream stream = new FileStream(genPath, FileMode.Create))
            {
                var writer = new BinaryWriter(stream);
                //Play file use UTF-8 encoding
                //Write play file header
                writer.Write(PlayDataHelper.GetStringBytes("BG"));
                //Write play file version
                writer.Write(PlayDataHelper.GetStringBytes(VersionInfo.PlayVersion));
                //Write play file data
                Compile();
                writer.Write(file.GeneratePlayData().ToArray());
            }
            MessageBox.Show((string)FindResource("PlayFileSavedStr"), (string)FindResource("TipTitleStr"),
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        void Compile()
        {
            foreach (var particleSystem in file.ParticleSystems)
            {
                foreach (var layer in particleSystem.Layers)
                {
                    foreach (var component in layer.Components)
                    {
                        CompilePropertyExpressions(component);
                        CompileEventGroups(component);
                    }
                }
            }
        }
        void CompilePropertyExpressions(PropertyContainer container)
        {
            if (container is Emitter)
                CompilePropertyExpressions((container as Emitter).Particle);

            Type containerType = container.GetType();
            foreach (var property in container.Properties)
            {
                if (property.Value.Expression)
                {
                    var lexer = new Expression.Lexer();
                    lexer.Load(property.Value.Value);
                    var syntaxTree = new Expression.Parser(lexer).Expression();
                    if (syntaxTree.ContainType<Expression.Name>() || syntaxTree.ContainType<Expression.Call>())
                    {
                        var compiledBytes = new List<byte>();
                        syntaxTree.Compile(compiledBytes);
                        property.Value.CompiledExpression = compiledBytes.ToArray();
                    }
                    else
                    {
                        object value = syntaxTree.Eval(null);
                        containerType.GetProperty(property.Key).GetSetMethod().Invoke(container, new object[] { value });
                    }
                }
            }
        }
        void CompileEventGroups(Component component)
        {
            CompileEvents(component.ComponentEventGroups);
            if (component is Emitter)
                CompileEvents((component as Emitter).ParticleEventGroups);
            else if (component is EventField)
                CompileEvents((component as EventField).EventFieldEventGroups);
            else if (component is Rebounder)
                CompileEvents((component as Rebounder).RebounderEventGroups);
        }
        void CompileEvents(IList<EventGroup> eventGroups)
        {
            foreach (EventGroup eventGroup in eventGroups)
            {
                if (!string.IsNullOrEmpty(eventGroup.Condition))
                {
                    var lexer = new Expression.Lexer();
                    lexer.Load(eventGroup.Condition);
                    var syntaxTree = new Expression.Parser(lexer).Expression();
                    var compiledBytes = new List<byte>();
                    syntaxTree.Compile(compiledBytes);
                    eventGroup.CompiledCondition = compiledBytes.ToArray();
                }
                eventGroup.CompiledEvents.Clear();
                foreach (string originalEvent in eventGroup.OriginalEvents)
                    eventGroup.CompiledEvents.Add(EventHelper.GenerateEventData(originalEvent, (t) =>
                        {
                            var lexer = new Expression.Lexer();
                            lexer.Load(t);
                            var syntaxTree = new Expression.Parser(lexer).Expression();
                            var compiledBytes = new List<byte>();
                            syntaxTree.Compile(compiledBytes);
                            return compiledBytes.ToArray();
                        }));
            }
        }
        void PlayCurrent()
        {
            if (!System.IO.File.Exists(config.PlayerPath))
            {
                MessageBox.Show((string)FindResource("PlayerNotFoundStr"), (string)FindResource("TipTitleStr"),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string genPath = Path.GetDirectoryName(config.PlayerPath);
            if (string.IsNullOrEmpty(genPath))
                genPath = Environment.CurrentDirectory;

            genPath += "\\Temp\\Temp.bg";
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(genPath)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(genPath));

            if (System.IO.File.Exists(genPath))
                System.IO.File.Delete(genPath);

            using (FileStream stream = new FileStream(genPath, FileMode.CreateNew))
            {
                var writer = new BinaryWriter(stream);
                //Play file use UTF-8 encoding
                //Write play file header
                writer.Write(PlayDataHelper.GetStringBytes("BG"));
                //Write play file version
                writer.Write(PlayDataHelper.GetStringBytes(VersionInfo.PlayVersion));
                //Write play file data
                Compile();
                writer.Write(file.GeneratePlayData().ToArray());
            }
            int particleSystemIndex = 0;
            for (int i = 0; i < file.ParticleSystems.Count; ++i)
            {
                if (selectedSystem == file.ParticleSystems[i])
                {
                    particleSystemIndex = i;
                    break;
                }
            }
            ProcessStartInfo ps = new ProcessStartInfo(config.PlayerPath);
            ps.Arguments = "\"" + genPath + "\" \"" + config.BackgroundPath + "\" " + particleSystemIndex + " ";
            ps.Arguments += config.ScreenWidth + " " + config.ScreenHeight + " ";
            ps.Arguments += config.ParticleMaximum + " " + config.CurveParticleMaximum + " ";
            ps.Arguments += config.Windowed + " ";
            ps.Arguments += config.ScreenCenter + " " + config.CenterX + " " + config.CenterY + " ";
            ps.Arguments += "\"" + config.SelfImagePath + "\" \"" + config.SelfSetting + "\"";
            ps.WindowStyle = ProcessWindowStyle.Normal;
            Process p = new Process();
            p.StartInfo = ps;
            p.Start();
            p.WaitForInputIdle();
        }
        void OpenPlaySetting()
        {
            Window window = new PlaySetting(config);
            window.ShowDialog();
            window.Close();
        }
        #endregion

        #region Window EventHandlers
        private void GeneratePlayFile_Click(object sender, RoutedEventArgs e)
        {
            GeneratePlayFile();
        }
        private void PlayItem_Click(object sender, RoutedEventArgs e)
        {
            PlayCurrent();
        }
        private void PlaySettingItem_Click(object sender, RoutedEventArgs e)
        {
            OpenPlaySetting();
        }
        #endregion
    }
}
