using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

public static class werlman
{
    [STAThread]
    static int Main()
    {
        Application.EnableVisualStyles();
        var desktopBounds = Screen.PrimaryScreen.Bounds;
        var window = new Form();
        window.StartPosition = FormStartPosition.CenterScreen;
        window.Width = desktopBounds.Width / 4 * 3;
        window.Height = desktopBounds.Height / 4 * 3;
        window.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        window.Text = "Erlang manual";
        var panel = new FlowLayoutPanel();
        panel.Dock = DockStyle.Top;
        panel.AutoSize = true;
        panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        var browser = new WebBrowser();
        browser.Dock = DockStyle.Fill;
        browser.Parent = window;
        panel.Parent = window;
        browser.TabStop = false;
        browser.ScriptErrorsSuppressed = true;
        var font = new Font("Consolas", 12);
        var moduleBox = new TextBox();
        moduleBox.MinimumSize = new Size(150, 20);
        moduleBox.Font = font;
        moduleBox.TextAlign = HorizontalAlignment.Right;
        moduleBox.Parent = panel;
        var separatorLabel = new Label();
        separatorLabel.AutoSize = true;
        separatorLabel.Dock = DockStyle.Fill;
        separatorLabel.TextAlign = ContentAlignment.MiddleCenter;
        separatorLabel.Font = new Font("Consolas", 14, FontStyle.Bold);
        separatorLabel.Text = ":";
        separatorLabel.Margin = new Padding(0);
        separatorLabel.Parent = panel;
        var functionBox = new TextBox();
        functionBox.MinimumSize = new Size(150, 20);
        functionBox.Font = font;
        functionBox.Parent = panel;
        var linkLabel = new Label();
        linkLabel.ForeColor = SystemColors.Highlight;
        var linkFont = new Font("Consolas", 12, FontStyle.Underline);
        linkLabel.Font = font;
        linkLabel.Cursor = Cursors.Hand;
        linkLabel.Parent = panel;
        linkLabel.AutoSize = true;
        linkLabel.Dock = DockStyle.Fill;
        linkLabel.TextAlign = ContentAlignment.MiddleCenter;
        linkLabel.MouseEnter += (sender, e) =>
        {
            linkLabel.Font = linkFont;
        };
        linkLabel.MouseLeave += (sender, e) =>
        {
            linkLabel.Font = font;
        };
        linkLabel.Click += (sender, e) =>
        {
            browser.Url = new Uri(linkLabel.Text);
        };
        var onTextChanged = new EventHandler((sender, e) =>
        {
            var moduleName = moduleBox.Text;
            var functionName = functionBox.Text.Replace('/', '-');
            var link = string.Format("http://www.erlang.org/doc/man/{0}.html#{1}",
                                     moduleName,
                                     functionName);
            var uri = Uri.EscapeUriString(link);
            linkLabel.Text = uri;
            browser.Url = new Uri(link);
        });
        moduleBox.TextChanged += onTextChanged;
        functionBox.TextChanged += onTextChanged;
        moduleBox.Text = "erlang";
        window.ShowDialog();
        return 0;
    }
}
