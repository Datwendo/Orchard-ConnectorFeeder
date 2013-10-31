using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Datwendo.ConnectorFeeder {
    public class Migrations : DataMigrationImpl {

        public int Create() 
        {         
            SchemaBuilder.CreateTable("ConnectorFeederPartRecord", table => table
                .ContentPartRecord()
                .Column<int>("PublisherId", c => c.WithDefault(0))
                .Column<string>("DataReceived", c => c.Nullable().WithLength(4096))
                );

            ContentDefinitionManager.AlterPartDefinition("ConnectorFeederPart",
                builder => builder.Attachable());

            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("ConnectorFeederPartRecord", table => table
                    .DropColumn("PartName")
                    );
            SchemaBuilder.AlterTable("ConnectorFeederPartRecord", table => table
                    .DropColumn("PropertyName")
                    );
            SchemaBuilder.AlterTable("ConnectorFeederPartRecord", table => table
                    .AddColumn<int>("PublisherId", c => c.WithDefault(0)));
            SchemaBuilder.AlterTable("ConnectorFeederPartRecord", table => table
                    .AddColumn<string>("DataReceived", c => c.Nullable().WithLength(4096)));
            return 2;
        }

    }
}