namespace PrecompiledExtensions
{
    public class Enums
    {
        // xxxx|xxxx|xxxx|xxxx
        // first is id, second is path, third is tier, fourth is skill
        public const int SKIER_TYPE_PATH_MASK   = 0x0F00;
        public const int SKIER_TYPE_TIER_MASK   = 0x00F0;
        public const int SKIER_TYPE_ID_MASK     = 0xF000;
        public const int SKIER_TYPE_SKILL_MASK  = 0x000F;

        public const int SKIER_TYPE_PATH_SHIFT  = 08;
        public const int SKIER_TYPE_TIER_SHIFT  = 04;
        public const int SKIER_TYPE_ID_SHIFT    = 12;
        public const int SKIER_TYPE_SKILL_SHIFT = 00;

        public enum SeatOccupency
        {//IDs are 1 indexed so empty can be 0
            EMPTY         = 0x0000,
            W_LIABILITIES = 0x1220,
            SS_LOVERS     = 0x2101,
            SC_KIDS       = 0x3202,
            R_TEACHERS    = 0x4023,
            N_NEWBIES     = 0x5011,
            OS_SNOWBIRDS  = 0x6032,
            H_CLOUDS      = 0x7133,
            R_STARS       = 0x8122,
            NF_SNOW       = 0x9000,
            SNOWPLOWS     = 0xA110,
            B_ADVENTURES  = 0xB213,
            S_SPECIALISTS = 0xC231,
        }

        public enum TowerPlacementAlgorithmType
        {
            D2,
            D3
        }
    }
}
