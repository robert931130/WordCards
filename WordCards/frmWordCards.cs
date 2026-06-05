namespace WordCards;

public class frmWordCards : Form
{
    private readonly WordCollection _WordList = new();
    private readonly string strWordFile = Path.Combine(AppContext.BaseDirectory, "WordCards.txt");
    private readonly ListBox lstWordList = new();
    private readonly PictureBox picLogo = new();
    private readonly Button btnAutoPlay = new();
    private readonly Label lblHelp = new();
    private readonly Label lblMessage = new();
    private readonly TextBox txtWord = new();
    private readonly TextBox txtPhonogram = new();
    private readonly TextBox txtExplain = new();
    private readonly StatusStrip ssrWord = new();
    private readonly ToolStripStatusLabel tsslMessage = new();
    private readonly System.Windows.Forms.Timer timPlayer = new();

    private dynamic? wmp;
    private bool isPlay;

    public frmWordCards()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "WordCards";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(900, 560);
        MinimumSize = new Size(760, 460);
        BackColor = Color.FromArgb(255, 254, 242);
        Font = new Font("Microsoft JhengHei UI", 10F);
        KeyPreview = true;
        string iconPath = Path.Combine(AppContext.BaseDirectory, "WordCards.ico");
        if (File.Exists(iconPath))
        {
            Icon = new Icon(iconPath);
        }

        lstWordList.Dock = DockStyle.Left;
        lstWordList.Width = 210;
        lstWordList.BorderStyle = BorderStyle.None;
        lstWordList.Font = new Font("Microsoft Sans Serif", 12F);
        lstWordList.BackColor = Color.FromArgb(206, 221, 239);
        lstWordList.ForeColor = Color.FromArgb(64, 64, 64);
        lstWordList.IntegralHeight = false;
        lstWordList.Click += lstWordList_Click;
        lstWordList.DoubleClick += lstWordList_DoubleClick;

        Panel palMain = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(255, 254, 242),
            Padding = new Padding(24, 18, 24, 18)
        };

        picLogo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        picLogo.Location = new Point(560, 20);
        picLogo.Size = new Size(86, 104);
        picLogo.SizeMode = PictureBoxSizeMode.Zoom;
        picLogo.Image = LoadLogo();

        btnAutoPlay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAutoPlay.Location = new Point(555, 138);
        btnAutoPlay.Size = new Size(96, 40);
        btnAutoPlay.Text = "Play";
        btnAutoPlay.BackColor = SystemColors.HotTrack;
        btnAutoPlay.ForeColor = Color.White;
        btnAutoPlay.FlatStyle = FlatStyle.Flat;
        btnAutoPlay.Click += btnAutoPlay_Click;

        lblHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblHelp.AutoSize = true;
        lblHelp.ForeColor = Color.Green;
        lblHelp.Text = "Enter: 下一個   Space: 重播   Double Click: 編輯";

        txtWord.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtWord.Location = new Point(24, 30);
        txtWord.Size = new Size(500, 64);
        txtWord.BorderStyle = BorderStyle.None;
        txtWord.BackColor = Color.FromArgb(255, 254, 242);
        txtWord.Font = new Font("Microsoft JhengHei UI", 28F, FontStyle.Bold);
        txtWord.ForeColor = SystemColors.HotTrack;
        txtWord.ReadOnly = true;

        txtPhonogram.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtPhonogram.Location = new Point(26, 105);
        txtPhonogram.Size = new Size(490, 38);
        txtPhonogram.BorderStyle = BorderStyle.None;
        txtPhonogram.BackColor = Color.FromArgb(255, 254, 242);
        txtPhonogram.Font = new Font("Microsoft JhengHei UI", 16F);
        txtPhonogram.ForeColor = Color.Green;
        txtPhonogram.ReadOnly = true;

        txtExplain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtExplain.Location = new Point(24, 190);
        txtExplain.Size = new Size(628, 285);
        txtExplain.BorderStyle = BorderStyle.None;
        txtExplain.BackColor = Color.FromArgb(255, 254, 242);
        txtExplain.Font = new Font("Microsoft JhengHei UI", 14F);
        txtExplain.ForeColor = Color.FromArgb(64, 64, 64);
        txtExplain.Multiline = true;
        txtExplain.ReadOnly = true;
        txtExplain.ScrollBars = ScrollBars.Vertical;

        palMain.Controls.Add(picLogo);
        palMain.Controls.Add(btnAutoPlay);
        palMain.Controls.Add(lblHelp);
        palMain.Controls.Add(txtWord);
        palMain.Controls.Add(txtPhonogram);
        palMain.Controls.Add(txtExplain);
        palMain.Resize += (_, _) => LayoutMainPanel(palMain);

        Panel palMessage = new()
        {
            Dock = DockStyle.Bottom,
            Height = 42,
            BackColor = Color.FromArgb(206, 221, 239)
        };
        lblMessage.Dock = DockStyle.Fill;
        lblMessage.TextAlign = ContentAlignment.MiddleLeft;
        lblMessage.Padding = new Padding(12, 0, 0, 0);
        lblMessage.ForeColor = Color.FromArgb(64, 64, 64);
        palMessage.Controls.Add(lblMessage);

        ssrWord.Dock = DockStyle.Bottom;
        ssrWord.Items.Add(tsslMessage);
        tsslMessage.Text = "請開啟單字檔";

        timPlayer.Interval = 2000;
        timPlayer.Tick += timPlayer_Tick;

        Controls.Add(palMain);
        Controls.Add(lstWordList);
        Controls.Add(palMessage);
        Controls.Add(ssrWord);

        Load += frmWordCards_Load;
        KeyPress += frmWordCards_KeyPress;
        FormClosing += frmWordCards_FormClosing;
    }

    private void LayoutMainPanel(Panel palMain)
    {
        int rightX = Math.Max(300, palMain.ClientSize.Width - 120);
        picLogo.Location = new Point(rightX, 20);
        btnAutoPlay.Location = new Point(rightX - 5, 138);
        lblHelp.Location = new Point(
            Math.Max(24, palMain.ClientSize.Width - lblHelp.Width - 24),
            Math.Max(18, palMain.ClientSize.Height - lblHelp.Height - 18));

        int textRight = Math.Max(260, picLogo.Left - 28);
        txtWord.Width = textRight - txtWord.Left;
        txtPhonogram.Width = textRight - txtPhonogram.Left;
        txtExplain.Width = palMain.ClientSize.Width - txtExplain.Left - 24;
        txtExplain.Height = Math.Max(120, lblHelp.Top - txtExplain.Top - 16);
    }

    private void frmWordCards_Load(object? sender, EventArgs e)
    {
        InitializePlayer();

        if (!File.Exists(strWordFile))
        {
            MessageBox.Show($"找不到單字檔\n{strWordFile}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            return;
        }

        string[] lines = File.ReadAllLines(strWordFile, System.Text.Encoding.Unicode);
        _WordList.LoadFromStringArray(lines);

        if (_WordList.Count > 0)
        {
            UpdateWordList();
            lstWordList.SelectedIndex = 0;
            ShowWord(_WordList[0]);
            lblMessage.Text = $"單字數量：{_WordList.Count}";
            tsslMessage.Text = "請選擇單字或按 Play";
        }
    }

    private void InitializePlayer()
    {
        try
        {
            Type? playerType = Type.GetTypeFromProgID("WMPlayer.OCX");
            if (playerType != null)
            {
                wmp = Activator.CreateInstance(playerType) ?? throw new InvalidOperationException("Windows Media Player is unavailable.");
                wmp.settings.autoStart = false;
                wmp.settings.mute = false;
            }
        }
        catch
        {
            wmp = null;
            tsslMessage.Text = "無法建立 Windows Media Player";
        }
    }

    private void ShowWord(WordItem word)
    {
        txtWord.Text = word.Word;
        txtPhonogram.Text = word.Phonogram;
        txtExplain.Text = word.Explain;
    }

    private void UpdateWordList()
    {
        lstWordList.BeginUpdate();
        lstWordList.Items.Clear();

        foreach (WordItem item in _WordList)
        {
            lstWordList.Items.Add(item);
        }

        lstWordList.EndUpdate();
    }

    private void PlayWord(WordItem word)
    {
        string soundPath = Path.IsPathRooted(word.SoundPath)
            ? word.SoundPath
            : Path.Combine(AppContext.BaseDirectory, word.SoundPath);

        if (!File.Exists(soundPath))
        {
            tsslMessage.Text = $"找無 {word.SoundPath} 音效檔";
            return;
        }

        if (wmp == null)
        {
            tsslMessage.Text = "Windows Media Player 無法使用";
            return;
        }

        try
        {
            wmp.URL = soundPath;
            wmp.controls.play();
            tsslMessage.Text = $"播放：{word.Word}";
        }
        catch (Exception ex)
        {
            tsslMessage.Text = $"播放失敗：{ex.Message}";
        }
    }

    private void PlaySelectedWord()
    {
        if (lstWordList.SelectedIndex < 0 || lstWordList.SelectedIndex >= _WordList.Count)
        {
            return;
        }

        WordItem word = _WordList[lstWordList.SelectedIndex];
        ShowWord(word);
        PlayWord(word);
    }

    private void NextWordList()
    {
        if (lstWordList.Items.Count == 0)
        {
            return;
        }

        lstWordList.Focus();
        lstWordList.SelectedIndex = lstWordList.SelectedIndex + 1 >= lstWordList.Items.Count
            ? 0
            : lstWordList.SelectedIndex + 1;

        int visibleRows = Math.Max(1, lstWordList.Height / Math.Max(1, lstWordList.ItemHeight));
        if (lstWordList.SelectedIndex >= visibleRows / 2)
        {
            lstWordList.TopIndex = Math.Max(0, lstWordList.SelectedIndex - visibleRows / 2);
        }
    }

    private void lstWordList_Click(object? sender, EventArgs e)
    {
        if (isPlay)
        {
            btnAutoPlay.PerformClick();
        }

        PlaySelectedWord();
    }

    private void lstWordList_DoubleClick(object? sender, EventArgs e)
    {
        if (lstWordList.SelectedIndex < 0)
        {
            return;
        }

        if (isPlay)
        {
            btnAutoPlay.PerformClick();
        }

        int idx = lstWordList.SelectedIndex;
        using frmEditWord edit = new(_WordList[idx]);
        DialogResult result = edit.ShowDialog(this);

        if (result == DialogResult.Yes)
        {
            UpdateWordList();
            lstWordList.SelectedIndex = idx;
            PlaySelectedWord();
            _WordList.SaveToFile(strWordFile);
            tsslMessage.Text = "單字已儲存";
        }
    }

    private void timPlayer_Tick(object? sender, EventArgs e)
    {
        NextWordList();
        PlaySelectedWord();
    }

    private void btnAutoPlay_Click(object? sender, EventArgs e)
    {
        lstWordList.Focus();

        if (!isPlay)
        {
            btnAutoPlay.Text = "Stop";
            isPlay = true;
            PlaySelectedWord();
            timPlayer.Start();
        }
        else
        {
            btnAutoPlay.Text = "Play";
            isPlay = false;
            timPlayer.Stop();
        }
    }

    private void frmWordCards_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (isPlay)
        {
            return;
        }

        switch (e.KeyChar)
        {
            case (char)Keys.Return:
                NextWordList();
                PlaySelectedWord();
                e.Handled = true;
                break;
            case (char)Keys.Space:
                PlaySelectedWord();
                e.Handled = true;
                break;
        }
    }

    private void frmWordCards_FormClosing(object? sender, FormClosingEventArgs e)
    {
        timPlayer.Stop();
    }

    private static Image LoadLogo()
    {
        string logoPath = Path.Combine(AppContext.BaseDirectory, "WordCards_Logo.png");
        if (File.Exists(logoPath))
        {
            return Image.FromFile(logoPath);
        }

        Bitmap bitmap = new(172, 208);
        using Graphics g = Graphics.FromImage(bitmap);
        g.Clear(Color.Transparent);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        using SolidBrush cardBrush = new(Color.White);
        using Pen borderPen = new(Color.FromArgb(74, 120, 180), 6);
        using SolidBrush accentBrush = new(SystemColors.HotTrack);
        using Font titleFont = new("Segoe UI", 32, FontStyle.Bold);
        using Font smallFont = new("Segoe UI", 18, FontStyle.Bold);

        Rectangle card = new(18, 14, 136, 176);
        g.FillRectangle(cardBrush, card);
        g.DrawRectangle(borderPen, card);
        g.FillRectangle(accentBrush, 18, 14, 136, 34);
        g.DrawString("W", titleFont, accentBrush, new PointF(56, 68));
        g.DrawString("CARD", smallFont, Brushes.Green, new PointF(48, 130));

        return bitmap;
    }
}
