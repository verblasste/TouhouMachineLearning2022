using System.Linq;
using System.Collections.Generic;
using TouhouMachineLearningSummary.GameEnum;

namespace TouhouMachineLearningSummary.CardSpace
{
    /// <summary>
    /// 卡牌名称:尼斯湖水怪
    /// 卡牌能力:我方回合结束：移除两侧单位能量值，对对方点数最大的单位造成等量伤害
    /// </summary>
    public class CardM_T0_1G_003 : Card
    {
        public override void Init()
        {
            //初始化通用卡牌效果
            base.Init();
            AbalityRegister(TriggerTime.When, TriggerType.Play)
               .AbilityAdd(async (e) =>
               {
                   await GameSystem.SelectSystem.SelectLocation(this, CardDeployTerritory, CardDeployRegion);
                   await GameSystem.TransferSystem.DeployCard(new Event(this, this));
               })
               .AbilityAppend();

            AbalityRegister(TriggerTime.When, TriggerType.TurnEnd)
              .AbilityAdd(async (e) =>
              {
                  int energyPoint = TwoSideCard.Sum(card => card[CardField.Energy]);
                  await GameSystem.FieldSystem.SetField(new Event(this, TwoSideCard).SetTargetField(CardField.Energy, 0));
                  await GameSystem.PointSystem.Hurt(new Event(this, GameSystem.InfoSystem.AgainstCardSet[GameRegion.Battle][Orientation.Op][CardFeature.LargestPointUnits].CardList).SetPoint(energyPoint));
              }, Condition.Default, Condition.OnMyRegion)
              .AbilityAppend();
        }
    }
}