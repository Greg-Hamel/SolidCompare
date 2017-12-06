using System.Collections.Generic;
using SolidCompare.Entities;

namespace SolidCompare.Comparators
{
    class MateComparator : IComparator<Mate>
    {
        private static MateComparator instance = null;

        private MateComparator()
        {

        }

        public static MateComparator Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new MateComparator();
                }
                return instance;
            }
        }

        private List<SldWorks.IMateEntity2> GetMateEntities(Mate mate)
        {
            List<SldWorks.IMateEntity2> result = new List<SldWorks.IMateEntity2>();

            for (int i = 0; i < mate.SwMate.GetMateEntityCount(); i++)
            {
                /*System.Console.WriteLine("------ Mate Entity : " + mate.SwMate.MateEntity(i) + " - Comp : " + mate.SwMate.MateEntity(i).ReferenceComponent.Name2
                    + " - Type : " + mate.SwMate.MateEntity(i).ReferenceType2 + " - Id : " + mate.SwMate.MateEntity(i).ReferenceComponent.GetID() + " - RefNAme : " + (((object)mate.SwMate.MateEntity(i).Reference).GetType().GetProperty("Name") == null));
                // System.Console.WriteLine(" - RefNAme : " + mate.SwMate.MateEntity(i).Reference + " - RefEntNAme : " + mate.SwMate.MateEntity(i).Reference.Name);*/
                result.Add(mate.SwMate.MateEntity(i));
            }

            return result;
        }

        // Ameliorations : Creer une entite MateEntity pour utiliser le ListComparator
        private CompareResultStatus CompareMateEntities(List<SldWorks.IMateEntity2> refMateEntities, List<SldWorks.IMateEntity2> modMateEntities)
        {
            CompareResultStatus result = CompareResultStatus.Identical;
            bool isDifferent = false;
            List<SldWorks.IMateEntity2> removed = new List<SldWorks.IMateEntity2>(refMateEntities);
            List<SldWorks.IMateEntity2> added = new List<SldWorks.IMateEntity2>(modMateEntities);

            foreach (SldWorks.IMateEntity2 refMateEntity in refMateEntities)
            {
                SldWorks.IMateEntity2 modMateEntity = modMateEntities.Find(x => x.ReferenceComponent.GetID() == refMateEntity.ReferenceComponent.GetID());

                if (null != modMateEntity)
                {
                    isDifferent = refMateEntity.ReferenceComponent.Name2 != modMateEntity.ReferenceComponent.Name2
                    || refMateEntity.ReferenceType2 != modMateEntity.ReferenceType2;

                    // SolidWorks API doesnt provide Name property for all IMateReferences
                    // || refMateEntity.Reference.Name != modMateEntity.Reference.Name;

                    removed.Remove(refMateEntity);
                    added.Remove(modMateEntity);
                }
            }

            if (added.Count > 0 || removed.Count > 0 || isDifferent)
            {
                result = CompareResultStatus.Different;
            }

            return result;
        }

        public CompareResult Compare(Mate refMate, Mate modMate)
        {
            CompareResultStatus status = CompareResultStatus.Identical;
            string details = "";
            List<SldWorks.IMateEntity2> refMateEntities = GetMateEntities(refMate);
            List<SldWorks.IMateEntity2> modMateEntities = GetMateEntities(modMate);

            bool isDifferent = refMate.SwMate.Type != modMate.SwMate.Type
                || refMate.SwMate.Alignment != modMate.SwMate.Alignment
                || refMate.SwMate.CanBeFlipped != modMate.SwMate.CanBeFlipped
                || refMate.SwMate.Flipped != modMate.SwMate.Flipped;
            /*
            System.Console.WriteLine("++++++++++" + refMate.SwMate.Type + " ++ " + modMate.SwMate.Type);
            System.Console.WriteLine("++++++++++" + refMate.SwMate.Alignment + " ++ " + modMate.SwMate.Alignment);
            System.Console.WriteLine("++++++++++" + refMate.SwMate.CanBeFlipped + " ++ " + modMate.SwMate.CanBeFlipped);
            System.Console.WriteLine("++++++++++" + refMate.SwMate.Flipped + " ++ " + modMate.SwMate.Flipped);
            */
            CompareResultStatus mateEntitiesStatus = CompareMateEntities(refMateEntities, modMateEntities);

            if (isDifferent || CompareResultStatus.Different == mateEntitiesStatus)
            {
                status = CompareResultStatus.Different;
                details = "Mate Type properties and/or Mate Entities Changed";
            }

            return new CompareResult(refMate.GetID(), status)
            {
                Details = details
            };
        }
    }
}
