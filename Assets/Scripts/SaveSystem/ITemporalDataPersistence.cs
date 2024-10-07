namespace SaveSystem
{
    public interface ITemporalDataPersistence
    {
        public void LoadTemporalData(TemporalDataSO temporalData);
        public void SaveTemporalData(TemporalDataSO temporalData);
    }
}