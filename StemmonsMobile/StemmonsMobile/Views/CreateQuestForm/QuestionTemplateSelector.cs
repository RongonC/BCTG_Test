using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StemmonsMobile.Views.CreateQuestForm
{
    public class QuestionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NotAnsweredTemplate { get; set; }

        public DataTemplate AnsweredTemplate { get; set; }

        public DataTemplate SubmittedTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //switch (((Question)item).State)
            //{
            //    case QuestionState.NotAnswered:
            //        return this.NotAnsweredTemplate;
            //    case QuestionState.Answered:
            //        return this.AnsweredTemplate;
            //    case QuestionState.Submitted:
            //        return this.SubmittedTemplate;
            //}
            throw null;
        }
    }
}

