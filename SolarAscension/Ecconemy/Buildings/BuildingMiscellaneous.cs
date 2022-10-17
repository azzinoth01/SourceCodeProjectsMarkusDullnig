public class BuildingMiscellaneous : Building {
    public BuildingMiscellaneous(BuildingDescription desc) : base(desc) {
    }

    public override void BuildingTick() {
        // throw new System.NotImplementedException();
    }

    public override void CheckPriority() {
        //throw new System.NotImplementedException();
    }

    public override bool CompleteBuildingPlacement() {
        //throw new System.NotImplementedException();

        return false;
    }

    public override bool CompleteBuildingRemoval() {
        // throw new System.NotImplementedException();
        return false;
    }
    public override RessourcesProduction GetProductionInformationPerMinute(bool useEfficiency = false, int index = 0) {
        return null;
    }
}
