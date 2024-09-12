using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Example1.Infrastructure.Contexts.BotPlatform.Migrations;

[DbContext(typeof(BotPlatformDbContext))]
[Migration("0002")]
public class M0002 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        const string sql = @"
CREATE TABLE IF NOT EXISTS `FilesBox` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `Data` longblob NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `Type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'None',
    `Create` datetime(6) NOT NULL,
    CONSTRAINT `PK_FilesBox` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE INDEX IF NOT EXISTS `IX_FilesBox_UserId` ON `FilesBox` (`UserId`);
";

        migrationBuilder.Sql(sql);
    }
}