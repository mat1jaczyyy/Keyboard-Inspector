﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Keyboard_Inspector {
    class Result: IBinary {
        static readonly char[] Header = new char[] { 'K', 'B', 'I', '\0' };
        static readonly uint FileVersion = 2;

        public string Title;
        public DateTime Recorded;

        double _time;
        public double Time {
            get => Recorder.Recording == this? Recorder.Elapsed : _time;
            set => _time = value;
        }
        public List<Event> Events;
        public Dictionary<long, Source> Sources;

        public Analysis Analysis;

        public List<InputInfo> Inputs { get; private set; }
        Dictionary<Input, InputInfo> Groups = new Dictionary<Input, InputInfo>();

        public Result()
        : this("", DateTime.Now, 0, new List<Event>(), new Dictionary<long, Source>()) {}

        public Result(string title, DateTime recorded, double time, List<Event> events, Dictionary<long, Source> sources, Analysis analysis = null, List<InputInfo> inputs = null) {
            // TODO Simplify?
            for (int i = 0; i < events.Count; i++) {
                var e = events[i];

                // Filter Windows auto-repeat
                if (Groups.ContainsKey(e.Input) && Groups[e.Input].Events.Last().Pressed == e.Pressed) {
                    events.RemoveAt(i);
                    i--;

                } else {
                    if (!Groups.ContainsKey(e.Input))
                        Groups[e.Input] = new InputInfo(e.Input);

                    Groups[e.Input].Events.Add(e);
                }
            }

            if (inputs == null) {
                inputs = Groups.Values.ToList();
            
            } else {
                foreach (var i in inputs) {
                    i.Events = Groups[i.Input].Events;
                }
            }
            
            Title = title?? "";
            Recorded = recorded;
            Time = time;
            Events = events;
            Sources = sources;
            Analysis = analysis?? new Analysis();
            Analysis.Result = this;
            Inputs = inputs;
        }

        public void Record(Event e) {
            if (Groups.ContainsKey(e.Input)) {
                if (Groups[e.Input].Events.Last().Pressed == e.Pressed)
                    return;

            } else if (!e.Pressed)
                return;

            Events.Add(e);

            if (!Groups.ContainsKey(e.Input))
                Inputs.Add(Groups[e.Input] = new InputInfo(e.Input));

            Groups[e.Input].Events.Add(e);

            if (!Sources.ContainsKey(e.Input.Source))
                Sources[e.Input.Source] = Source.FromHandle(e.Input.Source, 0);

            Sources[e.Input.Source].Count++;
        }

        public string GetTitle()
            => string.IsNullOrWhiteSpace(Title)? $"{GetBestSource(0.66)?.Name?? "Untitled"} on {Recorded:MM/dd/yyyy, h:mm:ss tt}" : Title;

        public Source GetBestSource(double threshold = 0) {
            Source best = null;

            foreach (var source in Sources.Values) {
                if (source.Count > (best?.Count ?? 0))
                    best = source;
            }

            if (best?.Count <= Events.Count * threshold)
                return null;

            return best;
        }

        public IEnumerable<InputInfo> VisibleInputs()
            => Inputs.Where(i => i.Visible);

        public HashSet<Input> VisibleInputSet()
            => VisibleInputs().Select(i => i.Input).ToHashSet();

        public int VisibleIndex(Input input) 
            => VisibleInputs().TakeWhile(i => i.Input != input).Count();

        public InputInfo GetInfo(Input input)
            => Inputs.Find(i => i.Input == input);

        public List<Event> AllVisibleEvents() {
            var visible = VisibleInputSet();
            return Events.Where(i => visible.Contains(i.Input)).ToList();
        }

        public void ToBinary(BinaryWriter bw) {
            bw.Write(Header);
            bw.Write(FileVersion);

            bw.Write(Title);
            bw.Write(Recorded.ToBinary());

            bw.Write(Time);
            Events.ToBinary(bw);
            Sources.ToBinary(bw);
            Analysis.ToBinary(bw);
            Inputs.ToBinary(bw);
        }

        static Dictionary<int, string> LegacySources = new Dictionary<int, string>() {
            {0, "Keyboard"},
            {1, "Gamepad"},
            {2, "Mouse"},
            {3, "TETR.IO Replay"}
        };

        public static Result FromBinary(BinaryReader br) {
            if (!br.ReadChars(Header.Length).SequenceEqual(Header))
                throw new Exception("Invalid header");

            uint fileVersion = br.ReadUInt32();

            string title = br.ReadString();
            DateTime recorded = DateTime.FromBinary(br.ReadInt64());
            double time = br.ReadDouble();
            List<Event> events = Utility.ListFromBinary(br, fileVersion, Event.FromBinary);

            Dictionary<long, Source> sources = new Dictionary<long, Source>();

            if (fileVersion == 0) {
                var srcs = events.Select(i => i.Input.Source);
                var uniques = srcs.Distinct().ToList();

                foreach (var entry in LegacySources) {
                    if (!uniques.Contains(entry.Key)) continue;
                    
                    sources.Add(entry.Key, new Source(srcs.Count(i => i == entry.Key), entry.Value));
                }

            } else {
                sources = Utility.DictionaryFromBinary(br, fileVersion, Source.FromBinary);
            }

            Analysis analysis = Analysis.FromBinary(br, fileVersion);

            List<InputInfo> inputs;

            if (fileVersion < 2) {
                inputs = null;

            } else {
                inputs = Utility.ListFromBinary(br, fileVersion, InputInfo.FromBinary);
            }

            return new Result(title, recorded, time, events, sources, analysis, inputs);
        }

        public static Result FromStream(Stream stream) {
            using (BinaryReader br = new BinaryReader(stream))
                return FromBinary(br);
        }

        public static bool IsEmpty(Result result) => result?.Events.Any() != true;
    }
}
