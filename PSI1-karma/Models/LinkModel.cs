// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Karma.Models
{
    public class LinkModel
    {
        public string Text { get; set; }
        public int Page { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Active { get; set; } = false;

        public LinkModel(int page)
        : this(page, true) { }

        public LinkModel(int page, bool enabled)
        : this(page, enabled, page.ToString())
        { }

        public LinkModel(int page, bool enabled, string text)
        {
            Page = page;
            Enabled = enabled;
            Text = text;
        }
    }
}
