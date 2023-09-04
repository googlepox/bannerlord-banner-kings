using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;
using SandBox.ViewModelCollection.Nameplate;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace BannerKings.UI.Extensions
{
    [ViewModelMixin("RefreshBindValues")]
    internal class SettlementNameplateVMMixin : BaseViewModelMixin<SettlementNameplateVM>
    {
        private readonly SettlementNameplateVM settlementNameplateVM;
        private bool _hasDisease;
        private int _diseaseIconYOffset;

        public SettlementNameplateVMMixin(SettlementNameplateVM vm)
          : base(vm)
        {
            this.settlementNameplateVM = vm;
            this.HasDisease = false;
            Settlement settlement = this.settlementNameplateVM.Settlement;
            if (settlement == null)
            {
                return;
            }

            if (settlement.IsTown)
            {
                this.DiseaseIconYOffset = 40;
            }
            else if (settlement.IsCastle)
            {
                this.DiseaseIconYOffset = 35;
            }
            else
            {
                this.DiseaseIconYOffset = 30;
            }
        }

        [DataSourceProperty]
        public int DiseaseIconYOffset
        {
            get => this._diseaseIconYOffset;
            set
            {
                if (value == this._diseaseIconYOffset)
                {
                    return;
                }

                this._diseaseIconYOffset = value;
                this.ViewModel.OnPropertyChangedWithValue(value, nameof(DiseaseIconYOffset));
            }
        }

        [DataSourceProperty]
        public bool HasDisease
        {
            get => this._hasDisease;
            set
            {
                if (value == this._hasDisease)
                {
                    return;
                }

                this._hasDisease = value;
                this.ViewModel.OnPropertyChangedWithValue(value, nameof(HasDisease));
            }
        }

        public override void OnRefresh()
        {
            if (!this.settlementNameplateVM.IsInRange)
            {
                return;
            }

            Settlement settlement = this.settlementNameplateVM.Settlement;
            this.HasDisease = false;
            if (BannerKingsConfig.Instance.PopulationManager.GetPopData(settlement).DiseaseData.ActiveDisease != null)
            {
                this.HasDisease = true;
            }
        }
    }
}
