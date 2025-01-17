//-----------------------------------------------------------------------
// <copyright file="SettingsDialog.cs" company="None">
//     Copyright (c) Brandon Wallace and Jesse Calhoun. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace TQVaultAE.GUI
{
	using System;
	using System.Drawing;
	using System.Globalization;
	using System.Linq;
	using System.Windows.Forms;
	using TQVaultAE.GUI.Models;
	using TQVaultAE.Data;
	using TQVaultAE.Presentation;
	using EnumsNET;
	using TQVaultAE.Domain.Results;

	/// <summary>
	/// Class for the Settings/Configuration Dialog
	/// </summary>
	internal partial class SettingsDialog : VaultForm
	{
		public string BaseFont { get; private set; }

		/// <summary>
		/// Indicates whether the title screen will be skipped on startup
		/// </summary>
		private bool skipTitle;

		/// <summary>
		/// Save path for the vault files
		/// </summary>
		public string VaultPath { get; private set; }

		/// <summary>
		/// Indicates whether item copying is allowed
		/// </summary>
		private bool allowItemCopy;

		/// <summary>
		/// Indicates whether item editing is allowed
		/// </summary>
		private bool allowItemEdit;

		/// <summary>
		/// Indicates whether character editing is allowed
		/// </summary>
		private bool allowCharacterEdit;

		/// <summary>
		/// Indicates whether the last opened character will be loaded at startup
		/// </summary>
		private bool loadLastCharacter;

		/// <summary>
		/// Indicates whether the last opened vault will be loaded at startup
		/// </summary>
		private bool loadLastVault;

		/// <summary>
		/// Indicates whether the language will be auto detected
		/// </summary>
		private bool detectLanguage;

		/// <summary>
		/// The language we will be using.
		/// </summary>
		private string titanQuestLanguage;

		/// <summary>
		/// Indicates whether the game paths will be auto detected
		/// </summary>
		private bool detectGamePath;

		/// <summary>
		/// Titan Quest game path
		/// </summary>
		private string titanQuestPath;

		/// <summary>
		/// Immortal Throne game path
		/// </summary>
		private string immortalThronePath;

		/// <summary>
		/// Indicates whether custom maps have been enabled
		/// </summary>
		private bool enableMods;

		/// <summary>
		/// Current custom map
		/// </summary>
		private string customMap;

		/// <summary>
		/// Indicates whether we are loading all data files on startup
		/// </summary>
		private bool loadAllFiles;

		/// <summary>
		/// Indicates whether warning messages are suppressed
		/// </summary>
		private bool suppressWarnings;

		/// <summary>
		/// Indicates whether player items will be readonly.
		/// </summary>
		private bool playerReadonly;

		/// <summary>
		/// Indicates whether the vault path has been changed
		/// </summary>
		public bool VaultPathChanged { get; private set; }

		/// <summary>
		/// Indicates whether the play list filter has been changed
		/// </summary>
		public bool PlayerFilterChanged { get; private set; }

		/// <summary>
		/// Indicates that any configuration item has been changed
		/// </summary>
		public bool ConfigurationChanged { get; private set; }

		/// <summary>
		/// Indicates that the UI setting has changed.
		/// </summary>
		public bool UISettingChanged { get; private set; }

		/// <summary>
		/// Indicates that the settings have been loaded
		/// </summary>
		private bool settingsLoaded;

		/// <summary>
		/// Indicates that the language setting has been changed
		/// </summary>
		public bool LanguageChanged { get; private set; }

		/// <summary>
		/// Indicates that the game language has been changed
		/// </summary>
		public bool GamePathChanged { get; private set; }

		/// <summary>
		/// Indicates that the custom map selection has changed
		/// </summary>
		public bool CustomMapsChanged { get; private set; }

		/// <summary>
		/// Initializes a new instance of the SettingsDialog class.
		/// </summary>
		public SettingsDialog(MainForm instance) : base(instance.ServiceProvider)
		{
			this.Owner = instance;

			this.InitializeComponent();

			#region Apply custom font

			this.characterEditCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.allowItemEditCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.allowItemCopyCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.skipTitleCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.loadLastCharacterCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.loadLastVaultCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.vaultPathTextBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.vaultPathLabel.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.cancelButton.Font = FontService.GetFontAlbertusMTLight(12F);
			this.okayButton.Font = FontService.GetFontAlbertusMTLight(12F);
			this.resetButton.Font = FontService.GetFontAlbertusMTLight(12F);
			this.vaultPathBrowseButton.Font = FontService.GetFontAlbertusMTLight(12F);
			this.enableCustomMapsCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.loadAllFilesCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.suppressWarningsCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.playerReadonlyCheckbox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.languageComboBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.languageLabel.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.detectLanguageCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.titanQuestPathTextBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.titanQuestPathLabel.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.immortalThronePathLabel.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.immortalThronePathTextBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.detectGamePathsCheckBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.titanQuestPathBrowseButton.Font = FontService.GetFontAlbertusMTLight(12F);
			this.immortalThronePathBrowseButton.Font = FontService.GetFontAlbertusMTLight(12F);
			this.customMapLabel.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.mapListComboBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.baseFontLabel.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.baseFontComboBox.Font = FontService.GetFontAlbertusMTLight(11.25F);
			this.Font = FontService.GetFontAlbertusMTLight(11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)(0));

			#endregion

			this.vaultPathLabel.Text = Resources.SettingsLabel1;
			this.languageLabel.Text = Resources.SettingsLabel2;
			this.titanQuestPathLabel.Text = Resources.SettingsLabel3;
			this.immortalThronePathLabel.Text = Resources.SettingsLabel4;
			this.customMapLabel.Text = Resources.SettingsLabel5;
			this.detectGamePathsCheckBox.Text = Resources.SettingsDetectGamePath;
			this.detectLanguageCheckBox.Text = Resources.SettingsDetectLanguage;
			this.enableCustomMapsCheckBox.Text = Resources.SettingsEnableMod;
			this.toolTip.SetToolTip(this.enableCustomMapsCheckBox, Resources.SettingsEnableModTT);
			this.skipTitleCheckBox.Text = Resources.SettingsSkipTitle;
			this.toolTip.SetToolTip(this.skipTitleCheckBox, Resources.SettingsSkipTitleTT);
			this.allowItemCopyCheckBox.Text = Resources.SettingsAllowCopy;
			this.toolTip.SetToolTip(this.allowItemCopyCheckBox, Resources.SettingsAllowCopyTT);
			this.allowItemEditCheckBox.Text = Resources.SettingsAllowEdit;
			this.toolTip.SetToolTip(this.allowItemEditCheckBox, Resources.SettingsAllowEdit);
			this.characterEditCheckBox.Text = Resources.SettingsAllowEditCE;
			this.toolTip.SetToolTip(this.characterEditCheckBox, Resources.SettingsAllowEditCE);
			this.loadLastCharacterCheckBox.Text = Resources.SettingsLoadChar;
			this.toolTip.SetToolTip(this.loadLastCharacterCheckBox, Resources.SettingsLoadCharTT);
			this.loadLastVaultCheckBox.Text = Resources.SettingsLoadVault;
			this.toolTip.SetToolTip(this.loadLastVaultCheckBox, Resources.SettingsLoadVaultTT);
			this.loadAllFilesCheckBox.Text = Resources.SettingsPreLoad;
			this.toolTip.SetToolTip(this.loadAllFilesCheckBox, Resources.SettingsPreLoadTT);
			this.suppressWarningsCheckBox.Text = Resources.SettingsNoWarning;
			this.toolTip.SetToolTip(this.suppressWarningsCheckBox, Resources.SettingsNoWarningTT);
			this.playerReadonlyCheckbox.Text = Resources.SettingsPlayerReadonly;
			this.toolTip.SetToolTip(this.playerReadonlyCheckbox, Resources.SettingsPlayerReadonlyTT);
			this.resetButton.Text = Resources.SettingsReset;
			this.toolTip.SetToolTip(this.resetButton, Resources.SettingsResetTT);
			this.cancelButton.Text = Resources.GlobalCancel;
			this.okayButton.Text = Resources.GlobalOK;
			this.Text = Resources.SettingsTitle;

			this.DrawCustomBorder = true;

			this.mapListComboBox.Items.Clear();

			var maps = GamePathResolver.GetCustomMapList();

			if (maps?.Any() ?? false)
				this.mapListComboBox.Items.AddRange(maps);

			if (!Config.Settings.Default.AllowCheats)
			{
				this.allowItemEditCheckBox.Visible = false;
				this.allowItemCopyCheckBox.Visible = false;
				this.characterEditCheckBox.Visible = false;
			}
		}

		/// <summary>
		/// Override of ScaleControl which supports font scaling.
		/// </summary>
		/// <param name="factor">SizeF for the scale factor</param>
		/// <param name="specified">BoundsSpecified value.</param>
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			this.Font = new Font(this.Font.FontFamily, this.Font.SizeInPoints * factor.Height, this.Font.Style);
			base.ScaleControl(factor, specified);
		}

		/// <summary>
		/// Handler for clicking the vault path browse button
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void VaultPathBrowseButtonClick(object sender, EventArgs e)
		{
			this.folderBrowserDialog.Description = Resources.SettingsBrowseVault;
			this.folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
			this.folderBrowserDialog.SelectedPath = this.VaultPath;
			this.folderBrowserDialog.ShowDialog();
			if (this.folderBrowserDialog.SelectedPath.Trim().ToUpperInvariant() != this.VaultPath.Trim().ToUpperInvariant())
			{
				this.VaultPath = this.folderBrowserDialog.SelectedPath.Trim().ToUpperInvariant();
				this.vaultPathTextBox.Text = this.VaultPath.Trim().ToUpperInvariant();
				this.vaultPathTextBox.Invalidate();
				this.ConfigurationChanged = true;
				this.VaultPathChanged = true;
			}
		}

		/// <summary>
		/// Handler for clicking the reset button
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void ResetButtonClick(object sender, EventArgs e)
		{
			if (this.ConfigurationChanged)
			{
				this.LoadSettings();
				this.UpdateDialogSettings();
				this.Invalidate();
			}
		}

		/// <summary>
		/// Handler for clicking the skip title check box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void SkipTitleCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.skipTitleCheckBox.Checked)
			{
				if (this.skipTitle == false)
				{
					this.skipTitle = true;
					this.ConfigurationChanged = true;
				}
			}
			else
			{
				if (this.skipTitle == true)
				{
					this.skipTitle = false;
					this.ConfigurationChanged = true;
				}
			}
		}

		/// <summary>
		/// Handler for loading this dialog box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void SettingsDialogLoad(object sender, EventArgs e)
		{
			this.settingsLoaded = false;

			this.LoadSettings();
			this.UpdateDialogSettings();
		}



		/// <summary>
		/// Loads the settings from the config file
		/// </summary>
		private void LoadSettings()
		{
			this.BaseFont = Config.Settings.Default.BaseFont;

			this.skipTitle = Config.Settings.Default.SkipTitle;
			this.VaultPath = Config.Settings.Default.VaultPath;
			this.allowItemCopy = Config.Settings.Default.AllowItemCopy;
			this.allowItemEdit = Config.Settings.Default.AllowItemEdit;
			this.allowCharacterEdit = Config.Settings.Default.AllowCharacterEdit;
			this.loadLastCharacter = Config.Settings.Default.LoadLastCharacter;
			this.loadLastVault = Config.Settings.Default.LoadLastVault;
			this.detectLanguage = Config.Settings.Default.AutoDetectLanguage;

			// Force English since there was some issue with getting the proper language setting.
			var gl = Database.GameLanguage;
			if (gl == null)
			{
				this.titanQuestLanguage = "English";
			}
			else
			{
				this.titanQuestLanguage = gl;
			}

			this.detectGamePath = Config.Settings.Default.AutoDetectGamePath;
			this.titanQuestPath = GamePathResolver.TQPath;
			this.immortalThronePath = GamePathResolver.ImmortalThronePath;
			this.enableMods = Config.Settings.Default.ModEnabled;
			this.customMap = Config.Settings.Default.CustomMap;
			this.loadAllFiles = Config.Settings.Default.LoadAllFiles;
			this.suppressWarnings = Config.Settings.Default.SuppressWarnings;
			this.playerReadonly = Config.Settings.Default.PlayerReadonly;

			this.settingsLoaded = true;
			this.ConfigurationChanged = false;
			this.VaultPathChanged = false;
			this.PlayerFilterChanged = false;
			this.LanguageChanged = false;
			this.GamePathChanged = false;
			this.UISettingChanged = false;
		}

		/// <summary>
		/// Updates the dialog settings display
		/// </summary>
		private void UpdateDialogSettings()
		{
			// Check to see that we can update things
			if (!this.settingsLoaded)
				return;

			// Build language combo box
			this.languageComboBox.Items.Clear();

			// Read the languages from the config file
			ComboBoxItem[] languages = Config.Settings.Default.GameLanguages.Split(',').Select(iso =>
			{
				CultureInfo ci = new CultureInfo(iso.ToUpperInvariant(), true);
				return new ComboBoxItem() { Value = ci.EnglishName, DisplayName = ci.DisplayName };// to keep EnglishName as baseline value
			}).OrderBy(cb => cb.DisplayName).ToArray();

			// Reading failed so we default to English
			if (!languages.Any()) languages = new ComboBoxItem[] { new ComboBoxItem() { Value = "English", DisplayName = "English" } };

			this.languageComboBox.Items.AddRange(languages);

			this.vaultPathTextBox.Text = this.VaultPath;
			this.skipTitleCheckBox.Checked = this.skipTitle;
			this.allowItemEditCheckBox.Checked = this.allowItemEdit;
			this.allowItemCopyCheckBox.Checked = this.allowItemCopy;
			this.characterEditCheckBox.Checked = this.allowCharacterEdit;
			this.loadLastCharacterCheckBox.Checked = this.loadLastCharacter;
			this.loadLastVaultCheckBox.Checked = this.loadLastVault;
			this.detectLanguageCheckBox.Checked = this.detectLanguage;
			this.detectGamePathsCheckBox.Checked = this.detectGamePath;
			this.immortalThronePathTextBox.Text = this.immortalThronePath;
			this.immortalThronePathTextBox.Enabled = !this.detectGamePath;
			this.immortalThronePathBrowseButton.Enabled = !this.detectGamePath;
			this.titanQuestPathTextBox.Text = this.titanQuestPath;
			this.titanQuestPathTextBox.Enabled = !this.detectGamePath;
			this.titanQuestPathBrowseButton.Enabled = !this.detectGamePath;
			this.loadAllFilesCheckBox.Checked = this.loadAllFiles;
			this.suppressWarningsCheckBox.Checked = this.suppressWarnings;
			this.playerReadonlyCheckbox.Checked = this.playerReadonly;

			this.enableCustomMapsCheckBox.Checked = this.enableMods;

			var lst = this.mapListComboBox.Items.Cast<GamePathEntry>();
			var found = lst.Where(m => m.Path == this.customMap).FirstOrDefault();
			this.mapListComboBox.SelectedItem = found;

			this.mapListComboBox.Enabled = this.enableMods;

			this.languageComboBox.SelectedItem = languages.FirstOrDefault(cb => cb.Value.Equals(this.titanQuestLanguage, StringComparison.InvariantCultureIgnoreCase));

			this.languageComboBox.Enabled = !this.detectLanguage;

			// Build Font combo box
			this.baseFontComboBox.Items.Clear();
			var listItem = Enums.GetMembers<FontFamilyList>()
				.Select(m => new ComboBoxItem()
				{
					Value = m.AsString(EnumFormat.Name),
					DisplayName = m.AsString(EnumFormat.Description, EnumFormat.Name)
				}).ToArray();
			this.baseFontComboBox.Items.AddRange(listItem);
			this.baseFontComboBox.SelectedItem = listItem.Where(i => i.Value == this.BaseFont).FirstOrDefault() ?? listItem.First();
		}

		/// <summary>
		/// Handler for clicking the OK button
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void OkayButtonClick(object sender, EventArgs e)
		{
			if (this.ConfigurationChanged)
			{
				Config.Settings.Default.SkipTitle = this.skipTitle;
				Config.Settings.Default.VaultPath = this.VaultPath;
				Config.Settings.Default.AllowItemCopy = this.allowItemCopy;
				Config.Settings.Default.AllowItemEdit = this.allowItemEdit;
				Config.Settings.Default.AllowCharacterEdit = this.allowCharacterEdit;
				Config.Settings.Default.LoadLastCharacter = this.loadLastCharacter;
				Config.Settings.Default.LoadLastVault = this.loadLastVault;
				Config.Settings.Default.AutoDetectLanguage = this.detectLanguage;
				Config.Settings.Default.TQLanguage = this.titanQuestLanguage;
				Config.Settings.Default.AutoDetectGamePath = this.detectGamePath;
				Config.Settings.Default.TQITPath = this.immortalThronePath;
				Config.Settings.Default.TQPath = this.titanQuestPath;
				Config.Settings.Default.ModEnabled = this.enableMods;
				Config.Settings.Default.CustomMap = this.customMap;
				Config.Settings.Default.LoadAllFiles = this.loadAllFiles;
				Config.Settings.Default.SuppressWarnings = this.suppressWarnings;
				Config.Settings.Default.PlayerReadonly = this.playerReadonly;
				Config.Settings.Default.BaseFont = this.BaseFont;
			}
		}

		/// <summary>
		/// Handler for clicking the allow item editing check box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void AllowItemEditCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.allowItemEditCheckBox.Checked)
			{
				if (this.allowItemEdit == false)
				{
					this.allowItemEdit = true;
					this.ConfigurationChanged = true;
				}
			}
			else
			{
				if (this.allowItemEdit == true)
				{
					this.allowItemEdit = false;
					this.ConfigurationChanged = true;
				}
			}
		}

		/// <summary>
		/// Handler for clicking the allow item copy check box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void AllowItemCopyCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.allowItemCopyCheckBox.Checked)
			{
				if (this.allowItemCopy == false)
				{
					this.allowItemCopy = true;
					this.ConfigurationChanged = true;
				}
			}
			else
			{
				if (this.allowItemCopy == true)
				{
					this.allowItemCopy = false;
					this.ConfigurationChanged = true;
				}
			}
		}

		/// <summary>
		/// Handler for clicking the load last character check box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void LoadLastCharacterCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.loadLastCharacterCheckBox.Checked)
			{
				if (this.loadLastCharacter == false)
				{
					this.loadLastCharacter = true;
					this.ConfigurationChanged = true;
				}
			}
			else
			{
				if (this.loadLastCharacter == true)
				{
					this.loadLastCharacter = false;
					this.ConfigurationChanged = true;
				}
			}
		}

		/// <summary>
		/// Handler for clicking the load last vault check box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void LoadLastVaultCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.loadLastVaultCheckBox.Checked)
			{
				if (this.loadLastVault == false)
				{
					this.loadLastVault = true;
					this.ConfigurationChanged = true;
				}
			}
			else
			{
				if (this.loadLastVault == true)
				{
					this.loadLastVault = false;
					this.ConfigurationChanged = true;
				}
			}
		}

		/// <summary>
		/// Handler for clicking the detect language check box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void DetectLanguageCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.detectLanguageCheckBox.Checked)
			{
				if (this.detectLanguage == false)
				{
					this.detectLanguage = true;
					this.languageComboBox.Enabled = false;
					this.ConfigurationChanged = true;

					// Force TQVault to restart to autodetect the language
					this.LanguageChanged = true;
				}
			}
			else
			{
				if (this.detectLanguage == true)
				{
					this.detectLanguage = false;
					this.languageComboBox.Enabled = true;
					this.ConfigurationChanged = true;
				}
			}
		}

		/// <summary>
		/// Handler for clicking the detect game paths check box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void DetectGamePathsCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.detectGamePathsCheckBox.Checked)
			{
				if (this.detectGamePath == false)
				{
					this.detectGamePath = true;
					this.immortalThronePathTextBox.Enabled = false;
					this.titanQuestPathTextBox.Enabled = false;
					this.titanQuestPathBrowseButton.Enabled = false;
					this.immortalThronePathBrowseButton.Enabled = false;
					this.ConfigurationChanged = true;

					// Force TQVault to restart to autodetect the game path
					this.GamePathChanged = true;
				}
			}
			else
			{
				if (this.detectGamePath == true)
				{
					this.detectGamePath = false;
					this.immortalThronePathTextBox.Enabled = true;
					this.titanQuestPathTextBox.Enabled = true;
					this.titanQuestPathBrowseButton.Enabled = true;
					this.immortalThronePathBrowseButton.Enabled = true;
					this.ConfigurationChanged = true;
				}
			}
		}

		/// <summary>
		/// Handler for changing the language selection
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void LanguageComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			// There was some problem getting the game language so we ignore changing it.
			var gl = Database.GameLanguage;
			if (gl == null)
				return;

			this.titanQuestLanguage = ((ComboBoxItem)this.languageComboBox.SelectedItem).Value;
			if (this.titanQuestLanguage.ToUpperInvariant() != gl.ToUpperInvariant())
				this.LanguageChanged = true;

			this.ConfigurationChanged = true;
		}

		/// <summary>
		/// Handler for leaving the vault text box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void VaultPathTextBoxLeave(object sender, EventArgs e)
		{
			if (this.VaultPath.ToUpperInvariant() != this.vaultPathTextBox.Text.Trim().ToUpperInvariant())
			{
				this.VaultPath = this.vaultPathTextBox.Text.Trim();
				this.vaultPathTextBox.Invalidate();
				this.ConfigurationChanged = true;
				this.VaultPathChanged = true;
			}
		}

		/// <summary>
		/// Handler for leaving the Titan Quest game path text box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void TitanQuestPathTextBoxLeave(object sender, EventArgs e)
		{
			if (this.titanQuestPath.ToUpperInvariant() != this.titanQuestPathTextBox.Text.Trim().ToUpperInvariant())
			{
				this.titanQuestPath = this.titanQuestPathTextBox.Text.Trim();
				this.titanQuestPathTextBox.Invalidate();
				this.ConfigurationChanged = true;
				this.GamePathChanged = true;
			}
		}

		/// <summary>
		/// Handler for leaving the Immortal Throne game path text box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void ImmortalThronePathTextBoxLeave(object sender, EventArgs e)
		{
			if (this.immortalThronePath.ToUpperInvariant() != this.immortalThronePathTextBox.Text.Trim().ToUpperInvariant())
			{
				this.immortalThronePath = this.immortalThronePathTextBox.Text.Trim();
				this.immortalThronePathTextBox.Invalidate();
				this.ConfigurationChanged = true;
				this.GamePathChanged = true;
			}
		}

		/// <summary>
		/// Handler for clicking the Titan Quest game path browse button
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void TitanQuestPathBrowseButtonClick(object sender, EventArgs e)
		{
			this.folderBrowserDialog.Description = Resources.SettingsBrowseTQ;
			this.folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			this.folderBrowserDialog.SelectedPath = this.titanQuestPath;
			this.folderBrowserDialog.ShowDialog();
			if (this.folderBrowserDialog.SelectedPath.Trim().ToUpperInvariant() != this.titanQuestPath.Trim().ToUpperInvariant())
			{
				this.titanQuestPath = this.folderBrowserDialog.SelectedPath.Trim().ToUpperInvariant();
				this.titanQuestPathTextBox.Text = this.titanQuestPath.Trim().ToUpperInvariant();
				this.titanQuestPathTextBox.Invalidate();
				this.ConfigurationChanged = true;
				this.GamePathChanged = true;
			}
		}

		/// <summary>
		/// Handler for clicking the Immortal Throne game path browse button
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void ImmortalThronePathBrowseButtonClick(object sender, EventArgs e)
		{
			this.folderBrowserDialog.Description = Resources.SettingsBrowseIT;
			this.folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			this.folderBrowserDialog.SelectedPath = this.immortalThronePath;
			this.folderBrowserDialog.ShowDialog();
			if (this.folderBrowserDialog.SelectedPath.Trim().ToUpperInvariant() != this.immortalThronePath.Trim().ToUpperInvariant())
			{
				this.immortalThronePath = this.folderBrowserDialog.SelectedPath.Trim().ToUpperInvariant();
				this.immortalThronePathTextBox.Text = this.immortalThronePath.Trim().ToUpperInvariant();
				this.immortalThronePathTextBox.Invalidate();
				this.ConfigurationChanged = true;
				this.GamePathChanged = true;
			}
		}

		/// <summary>
		/// Handler for clicking enable custom maps
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void EnableCustomMapsCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.enableCustomMapsCheckBox.Checked)
			{
				if (this.enableMods == false)
				{
					this.enableMods = true;
					this.ConfigurationChanged = true;
					this.CustomMapsChanged = true;
					this.mapListComboBox.Enabled = true;
				}
			}
			else
			{
				if (this.enableMods == true)
				{
					this.enableMods = false;
					this.ConfigurationChanged = true;
					this.CustomMapsChanged = true;
					this.customMap = string.Empty;// Reset value
					this.mapListComboBox.Enabled = false;
				}
			}
		}

		/// <summary>
		/// Handler for changing the selected custom map
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void MapListComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.settingsLoaded)
				return;

			var custommap = (this.mapListComboBox.SelectedItem as GamePathEntry)?.Path ?? string.Empty;
			if (custommap != Config.Settings.Default.CustomMap)
			{
				this.customMap = custommap;
				this.ConfigurationChanged = true;
				this.CustomMapsChanged = true;
			}
		}

		/// <summary>
		/// Handler for clicking the load all files check box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void LoadAllFilesCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.loadAllFilesCheckBox.Checked)
			{
				if (this.loadAllFiles == false)
				{
					this.loadAllFiles = true;
					this.ConfigurationChanged = true;
				}
			}
			else
			{
				if (this.loadAllFiles == true)
				{
					this.loadAllFiles = false;
					this.ConfigurationChanged = true;
				}
			}
		}

		/// <summary>
		/// Handler for clicking the suppress warnings check box
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="e">EventArgs data</param>
		private void SuppressWarningsCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.suppressWarningsCheckBox.Checked)
			{
				if (this.suppressWarnings == false)
				{
					this.suppressWarnings = true;
					this.ConfigurationChanged = true;
				}
			}
			else
			{
				if (this.suppressWarnings == true)
				{
					this.suppressWarnings = false;
					this.ConfigurationChanged = true;
				}
			}
		}

		private void PlayerReadonlyCheckboxCheckedChanged(object sender, EventArgs e)
		{
			if (this.playerReadonlyCheckbox.Checked)
			{
				if (this.playerReadonly == false)
				{
					this.playerReadonly = true;
					this.ConfigurationChanged = true;
				}
			}
			else
			{
				if (this.playerReadonly == true)
				{
					this.playerReadonly = false;
					this.ConfigurationChanged = true;
				}
			}

		}

		private void CharacterEditCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.characterEditCheckBox.Checked)
			{
				if (this.allowCharacterEdit == false)
				{
					this.allowCharacterEdit = true;
					this.ConfigurationChanged = true;
				}
			}
			else
			{
				if (this.allowCharacterEdit == true)
				{
					this.allowCharacterEdit = false;
					this.ConfigurationChanged = true;
				}
			}
		}

		private void FontComboBoxBase_SelectedIndexChanged(object sender, EventArgs e)
		{
			var font = this.baseFontComboBox.SelectedItem as ComboBoxItem;
			if (font == null)
				return;

			if (font.Value != Config.Settings.Default.BaseFont)
			{
				this.BaseFont = font.Value;
				this.ConfigurationChanged = true;

				this.UISettingChanged = true;// Force restart
			}
		}
	}
}