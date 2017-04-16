using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Tasky.Core;
using TaskyAndroid;
using Android.Views;

namespace TaskyAndroid.Screens {
	/// <summary>
	/// Main ListView screen displays a list of tasks, plus an [Add] button
	/// </summary>
	[Activity (Label = "Tasky", MainLauncher = true, Icon="@drawable/icon")]			
	public class HomeScreen : Activity {
		Adapters.TaskListAdapter taskList;
		IList<Task> tasks;
		Button addTaskButton;
		ListView taskListView;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// set our layout to be the home screen
			SetContentView(Resource.Layout.HomeScreen);

            View titleView = Window.FindViewById(Android.Resource.Id.Title);
            if (titleView != null)
            {
                IViewParent parent = titleView.Parent;
                if (parent != null && (parent is View))
                {
                    View parentView = (View)parent;
                    parentView.SetBackgroundColor(Android.Graphics.Color.Rgb(0x26, 0x75, 0xFF)); //38, 117 ,255
                }
            }

            //Find our controls
            taskListView = FindViewById<ListView> (Resource.Id.TaskList);
			addTaskButton = FindViewById<Button> (Resource.Id.AddButton);

			// wire up add task button handler
			if(addTaskButton != null) {
				addTaskButton.Click += (sender, e) => {
					StartActivity(typeof(TaskDetailsScreen));
				};
			}
			
			// wire up task click handler
			if(taskListView != null) {
				taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var taskDetails = new Intent (this, typeof (TaskDetailsScreen));
					taskDetails.PutExtra ("TaskID", tasks[e.Position].ID);
					StartActivity (taskDetails);
				};
			}
		}
		
		protected override void OnResume ()
		{
			base.OnResume ();

			tasks = TaskManager.GetTasks();
			
			// create our adapter
			taskList = new Adapters.TaskListAdapter(this, tasks);

			//Hook up our adapter to our ListView
			taskListView.Adapter = taskList;
		}
	}
}