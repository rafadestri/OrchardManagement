using OrchardManagement.Models;
using System.Linq;

namespace OrchardManagement.Data
{
    public class DbInitializer
    {
        public static void Initialize(OrchardContext context)
        {
            context.Database.EnsureCreated();

            if (context.Species.Any())
            {
                return;
            }

            var species = new Species[]
            {
                new Species { Description = "Butiazeiro " },
                new Species { Description = "Corticeira-da-serra" },
                new Species { Description = "Jerivá" },
                new Species { Description = "Pitangueira" },
                new Species { Description = "Cedro" },
                new Species { Description = "Pata-de-vaca" },
                new Species { Description = "Ingazeiro" },
                new Species { Description = "Corticeira-do-banhado" },
                new Species { Description = "Umbuzeiro" },
                new Species { Description = "Figueira" },
                new Species { Description = "Ipê-roxo" },
                new Species { Description = "Macieira" },
                new Species { Description = "Bergamoteira" },
                new Species { Description = "Laranjeira" }
            };

            context.Species.AddRange(species);
            context.SaveChanges();

            if (context.TreeGroup.Any())
            {
                return;
            }

            var treeGroups = new TreeGroup[]
            {
                new TreeGroup { Name = "Árvores", Description = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit."},
                new TreeGroup { Name = "Arbustos", Description = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit." },
                new TreeGroup { Name = "Coníferas", Description = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit." },
                new TreeGroup { Name = "Trepadeiras", Description = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit." },
                new TreeGroup { Name = "Palmeiras", Description = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit." },
                new TreeGroup { Name = "Suculentas", Description = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit." },
                new TreeGroup { Name = "Frutíferas", Description = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit." }
            };

            context.TreeGroup.AddRange(treeGroups);
            context.SaveChanges();
        }
    }
}
