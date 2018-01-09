using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
{
    /// <summary>
    /// Абстрактный класс описывает внутренне хранилищем данных клиента SAGA.
    /// </summary>
    public abstract class IntfInnerStore
    {

        public List<ContainerNode> containers = new List<ContainerNode>();

        /// <summary>
        /// Заполнение внутреннего хранилища.
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// Заполнение внутреннего хранилища.
        /// </summary>
        public abstract DocumentMap RefreshParagraphs(long contID, long docID);

        /// <summary>
        /// Заполнение Хранилища данными о контейнерах.
        /// </summary>
        /// <param name="list">Набор данных или список</param>
        /// <returns></returns>
        public abstract void FillContainers(ComplexValue list);

        /// <summary>
        /// Заполнение Хранилища данными о документах.
        /// </summary>
        /// <param name="list">Набор данных или список</param>
        /// <returns></returns>
        public abstract void FillDocs(ComplexValue list);

        /// <summary>
        /// Заполнение Документа данными о его абзацах.
        /// </summary>
        /// <param name="list">Набор данных или список</param>
        /// <returns></returns>
        public abstract void FillDocsParagraphs(ComplexValue list);

        /// <summary>
        /// Поиск контейнера в хранилище по его ID.
        /// </summary>
        /// <param name="ContainerID">ID контейнера</param>
        /// <returns>ContainerNode</returns>
        public ContainerNode GetContainerByID(long ContainerID)
        {
            var result = RecursGetContainerByID(containers, ContainerID);
            return result;
        }

        private ContainerNode RecursGetContainerByID(List<ContainerNode> containers, long ContainerID)
        {
            var result = containers.Where(x => x.ContainerID == ContainerID).FirstOrDefault();
            if (result == null)
                foreach (var cont in containers)
                {
                    var children = cont.Children();
                    result = RecursGetContainerByID(cont.Children(), ContainerID);
                    if (result != null)
                        break;
                }
            return result;
        }

    }
}

