namespace WordCards;

public class frmEditWord : Form
{
    private readonly TextBox txtWord = new();
    private readonly TextBox txtPhonogram = new();
    private readonly TextBox txtSoundPath = new();
    private readonly TextBox txtExplain = new();
    private readonly Button btnSave = new();
    private readonly Button btnCancel = new();

    public WordItem Word { get; }

    public frmEditWord(WordItem word)
    {
        Word = word;
        InitializeComponent();

        txtWord.Text = word.Word;
        txtPhonogram.Text = word.Phonogram;
        txtSoundPath.Text = word.SoundPath;
        txtExplain.Text = word.Explain;
    }

    private void InitializeComponent()
    {
        Text = "編輯單字";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(520, 420);
        MinimumSize = new Size(460, 360);
        Font = new Font("Microsoft JhengHei UI", 10F);
        FormBorderStyle = FormBorderStyle.Sizable;

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(14)
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

        layout.Controls.Add(CreateGroupBox("Word", txtWord), 0, 0);
        layout.Controls.Add(CreateGroupBox("Phonogram", txtPhonogram), 0, 1);
        layout.Controls.Add(CreateGroupBox("SoundPath", txtSoundPath), 0, 2);
        layout.Controls.Add(CreateGroupBox("Explain", txtExplain, true), 0, 3);

        FlowLayoutPanel buttons = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 10, 0, 0)
        };

        btnSave.Text = "儲存";
        btnSave.Width = 90;
        btnSave.DialogResult = DialogResult.Yes;
        btnSave.Click += btnSave_Click;

        btnCancel.Text = "取消";
        btnCancel.Width = 90;
        btnCancel.DialogResult = DialogResult.Cancel;

        buttons.Controls.Add(btnSave);
        buttons.Controls.Add(btnCancel);
        layout.Controls.Add(buttons, 0, 4);

        AcceptButton = btnSave;
        CancelButton = btnCancel;
        Controls.Add(layout);
    }

    private static GroupBox CreateGroupBox(string text, TextBox textBox, bool multiline = false)
    {
        GroupBox groupBox = new()
        {
            Text = text,
            Dock = DockStyle.Fill,
            Padding = new Padding(10, 22, 10, 10)
        };

        textBox.Dock = DockStyle.Fill;
        textBox.Multiline = multiline;
        textBox.ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None;

        groupBox.Controls.Add(textBox);
        return groupBox;
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        Word.Word = txtWord.Text.Trim();
        Word.Phonogram = txtPhonogram.Text.Trim();
        Word.SoundPath = txtSoundPath.Text.Trim();
        Word.Explain = txtExplain.Text.Trim();
    }
}
