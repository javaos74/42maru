using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using Chatbot._42Maru.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using RestSharp;
using Chatbot._42Maru.Models;
using Newtonsoft;

namespace Chatbot._42Maru.Activities
{
    [LocalizedDisplayName(nameof(Resources.SendText_DisplayName))]
    [LocalizedDescription(nameof(Resources.SendText_Description))]
    public class SendText : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.Timeout_DisplayName))]
        [LocalizedDescription(nameof(Resources.Timeout_Description))]
        public InArgument<int> TimeoutMS { get; set; } = 60000;

        [LocalizedDisplayName(nameof(Resources.SendText_Endpoint_DisplayName))]
        [LocalizedDescription(nameof(Resources.SendText_Endpoint_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [RequiredArgument]
        public InArgument<string> Endpoint { get; set; }

        [LocalizedDisplayName(nameof(Resources.SendText_ScenarioId_DisplayName))]
        [LocalizedDescription(nameof(Resources.SendText_ScenarioId_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [RequiredArgument]
        public InArgument<string> ScenarioId { get; set; }

        [LocalizedDisplayName(nameof(Resources.SendText_SessionId_DisplayName))]
        [LocalizedDescription(nameof(Resources.SendText_SessionId_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [RequiredArgument]
        public InArgument<string> SessionId { get; set; }

        [LocalizedDisplayName(nameof(Resources.SendText_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.SendText_Text_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [RequiredArgument]
        public InArgument<string> Text { get; set; }

        [LocalizedDisplayName(nameof(Resources.SendText_Result_DisplayName))]
        [LocalizedDescription(nameof(Resources.SendText_Result_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<bool> Result { get; set; }

        #endregion


        private Boolean status; 
        #region Constructors

        public SendText()
        {
            
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Endpoint == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Endpoint)));
            if (ScenarioId == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ScenarioId)));
            if (SessionId == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SessionId)));
            if (Text == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Text)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var timeout = TimeoutMS.Get(context);

            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
                Result.Set(ctx, status);
            };
        }

        private async Task ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {

            var endpoint = Endpoint.Get(context);
            var scenarioid = ScenarioId.Get(context);
            var sessionid = SessionId.Get(context);
            var text = Text.Get(context);
            ///////////////////////////
            // Add execution logic HERE
            ///////////////////////////
            var txtrply = new TextReply();
            txtrply.scenario_id = scenarioid;
            txtrply.session_id = sessionid;
            txtrply.messages.Add(new TextMessage( new TextData(text)));
            var client = new RestSharp.RestClient(endpoint);
            var request = new RestRequest("/api/integration/uipath/callback", Method.POST);
            client.AddDefaultHeader("Content-Type", "application/json");
            request.AddJsonBody( txtrply);
            var resp = client.Execute(request); 
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                status = true;
            } 
            else
            {
#if DEBUG
                Console.WriteLine($"status_code={resp.StatusCode}, data={resp.Content}");
#endif
                status = false;
            }


        }

        #endregion
    }
}

