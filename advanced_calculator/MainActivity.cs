using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Javax.Security.Auth;

namespace advanced_calculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TextView calcText;

        private string[] numbers = new string[2];
        private string @operator;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            calcText = FindViewById<TextView>(Resource.Id.calculator_txt_view);
        }
        [Java.Interop.Export("ButtonClick")] //xml onclick is defined in java
        public void ButtonClick(View v) //gets the button attributes from the xml
        {
            Button button = (Button)v;
            if ("0123456789.".Contains(button.Text))
                AddDigitOrDP(button.Text);
            else if ("÷-+x".Contains(button.Text))
                AddOperator(button.Text);
            else if ("=" == button.Text)
                calculate();
            else
                Erase();

        }
        private void AddDigitOrDP(string value)
        {
            int index = @operator == null ? 0 : 1;
            if (value == "." && numbers[index] == null)
                return;
            if (value == "." && numbers[index].Contains("."))  
                return;
            numbers[index] += value;

            UpdateCalcText();
        }
        private void AddOperator(string value)
        {
            if (numbers[1] != null)
            {
                calculate(value);
                return;
            }
            @operator = value;
            UpdateCalcText();
        }
        private void calculate(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);

            switch (@operator)
            {
                case "÷":
                    result = first / second;
                    break;
                case "x":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "-":
                    result = first - second;
                    break;
            }
            if (result != null)
            {
                numbers[0] = result.ToString();
                @operator = newOperator;
                numbers[1] = null;
                UpdateCalcText();
            }
        }
        public void Erase()
        {
            numbers[0] = null;
            numbers[1] = null;
            @operator = null;
            UpdateCalcText();
        }
        public void UpdateCalcText()
        {
            calcText.Text = $"{numbers[0]} {@operator} {numbers[1]}";
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
