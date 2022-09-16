using Microsoft.EntityFrameworkCore.Migrations;

namespace HassyaAllrightCloud.Infrastructure.Persistence.Migrations
{
    public partial class Init_Kobo_Database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineUser");

            migrationBuilder.DropTable(
                name: "LKD_Biko");

            migrationBuilder.DropTable(
                name: "LKD_Coupon");

            migrationBuilder.DropTable(
                name: "LKD_EtcImport");

            migrationBuilder.DropTable(
                name: "LKD_FutTum");

            migrationBuilder.DropTable(
                name: "LKD_Haiin");

            migrationBuilder.DropTable(
                name: "LKD_Haisha");

            migrationBuilder.DropTable(
                name: "LKD_HaishaExp");

            migrationBuilder.DropTable(
                name: "LKD_Henko");

            migrationBuilder.DropTable(
                name: "LKD_JitHou");

            migrationBuilder.DropTable(
                name: "LKD_Jyosha");

            migrationBuilder.DropTable(
                name: "LKD_Kaknin");

            migrationBuilder.DropTable(
                name: "LKD_Kariei");

            migrationBuilder.DropTable(
                name: "LKD_Kikyuj");

            migrationBuilder.DropTable(
                name: "LKD_Koban");

            migrationBuilder.DropTable(
                name: "LKD_Kotei");

            migrationBuilder.DropTable(
                name: "LKD_Koteik");

            migrationBuilder.DropTable(
                name: "LKD_MFutTu");

            migrationBuilder.DropTable(
                name: "LKD_Mihrim");

            migrationBuilder.DropTable(
                name: "LKD_Mishum");

            migrationBuilder.DropTable(
                name: "LKD_NyShCu");

            migrationBuilder.DropTable(
                name: "LKD_NyShmi");

            migrationBuilder.DropTable(
                name: "LKD_NyuSih");

            migrationBuilder.DropTable(
                name: "LKD_Shabni");

            migrationBuilder.DropTable(
                name: "LKD_Shuri");

            migrationBuilder.DropTable(
                name: "LKD_ShuYmd");

            migrationBuilder.DropTable(
                name: "LKD_Tehai");

            migrationBuilder.DropTable(
                name: "LKD_TokuYm");

            migrationBuilder.DropTable(
                name: "LKD_Unkobi");

            migrationBuilder.DropTable(
                name: "LKD_UnkobiExp");

            migrationBuilder.DropTable(
                name: "LKD_YFutTu");

            migrationBuilder.DropTable(
                name: "LKD_YMFuTu");

            migrationBuilder.DropTable(
                name: "LKD_Yousha");

            migrationBuilder.DropTable(
                name: "LKD_YouSyu");

            migrationBuilder.DropTable(
                name: "LKD_YykReport");

            migrationBuilder.DropTable(
                name: "LKD_YykReportMei");

            migrationBuilder.DropTable(
                name: "LKD_Yyksho");

            migrationBuilder.DropTable(
                name: "LKD_YykSyu");

            migrationBuilder.DropTable(
                name: "TKD_Biko");

            migrationBuilder.DropTable(
                name: "TKD_BookingMaxMinFareFeeCalc");

            migrationBuilder.DropTable(
                name: "TKD_BookingMaxMinFareFeeCalcMeisai");

            migrationBuilder.DropTable(
                name: "TKD_Calend");

            migrationBuilder.DropTable(
                name: "TKD_Coupon");

            migrationBuilder.DropTable(
                name: "TKD_EtcImport");

            migrationBuilder.DropTable(
                name: "TKD_FavoriteMenu");

            migrationBuilder.DropTable(
                name: "TKD_FavoriteSite");

            migrationBuilder.DropTable(
                name: "TKD_FutTum");

            migrationBuilder.DropTable(
                name: "TKD_Haiin");

            migrationBuilder.DropTable(
                name: "TKD_HaiinMail");

            migrationBuilder.DropTable(
                name: "TKD_Haisha");

            migrationBuilder.DropTable(
                name: "TKD_HaishaExp");

            migrationBuilder.DropTable(
                name: "TKD_Henko");

            migrationBuilder.DropTable(
                name: "TKD_JitHou");

            migrationBuilder.DropTable(
                name: "TKD_Jyosha");

            migrationBuilder.DropTable(
                name: "TKD_Kaknin");

            migrationBuilder.DropTable(
                name: "TKD_Kariei");

            migrationBuilder.DropTable(
                name: "TKD_Kikyuj");

            migrationBuilder.DropTable(
                name: "TKD_Koban");

            migrationBuilder.DropTable(
                name: "TKD_Kotei");

            migrationBuilder.DropTable(
                name: "TKD_Koteik");

            migrationBuilder.DropTable(
                name: "TKD_Kuri");

            migrationBuilder.DropTable(
                name: "TKD_LockTable");

            migrationBuilder.DropTable(
                name: "TKD_MFutTu");

            migrationBuilder.DropTable(
                name: "TKD_Mihrim");

            migrationBuilder.DropTable(
                name: "TKD_Mishum");

            migrationBuilder.DropTable(
                name: "TKD_Notice");

            migrationBuilder.DropTable(
                name: "TKD_NyShCu");

            migrationBuilder.DropTable(
                name: "TKD_NyShmi");

            migrationBuilder.DropTable(
                name: "TKD_NyuSih");

            migrationBuilder.DropTable(
                name: "TKD_PersonalNote");

            migrationBuilder.DropTable(
                name: "TKD_SchCusGrp");

            migrationBuilder.DropTable(
                name: "TKD_SchCusGrpMem");

            migrationBuilder.DropTable(
                name: "TKD_SchHyoSet");

            migrationBuilder.DropTable(
                name: "TKD_SchYotei");

            migrationBuilder.DropTable(
                name: "TKD_SchYotKSya");

            migrationBuilder.DropTable(
                name: "TKD_Seikyu");

            migrationBuilder.DropTable(
                name: "TKD_SeiMei");

            migrationBuilder.DropTable(
                name: "TKD_SeiPrS");

            migrationBuilder.DropTable(
                name: "TKD_SeiUch");

            migrationBuilder.DropTable(
                name: "TKD_Shabni");

            migrationBuilder.DropTable(
                name: "TKD_Shuri");

            migrationBuilder.DropTable(
                name: "TKD_ShuYmd");

            migrationBuilder.DropTable(
                name: "TKD_Tehai");

            migrationBuilder.DropTable(
                name: "TKD_TokuYm");

            migrationBuilder.DropTable(
                name: "TKD_Unkobi");

            migrationBuilder.DropTable(
                name: "TKD_UnkobiExp");

            migrationBuilder.DropTable(
                name: "TKD_YFutTu");

            migrationBuilder.DropTable(
                name: "TKD_YMFuTu");

            migrationBuilder.DropTable(
                name: "TKD_Yousha");

            migrationBuilder.DropTable(
                name: "TKD_YouSyu");

            migrationBuilder.DropTable(
                name: "TKD_YykReport");

            migrationBuilder.DropTable(
                name: "TKD_YykReportMei");

            migrationBuilder.DropTable(
                name: "TKD_Yyksho");

            migrationBuilder.DropTable(
                name: "TKD_YykSyu");

            migrationBuilder.DropTable(
                name: "TKM_JisKin");

            migrationBuilder.DropTable(
                name: "TKM_KasSet");
        }
    }
}
