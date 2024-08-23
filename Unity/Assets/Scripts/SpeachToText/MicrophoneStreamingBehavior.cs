using NativeWebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEditor;
using UnityEngine;
// post request 
using UnityEngine.Networking;

public class MicrophoneStreamingBehavior : MonoBehaviour
{
    WebSocket websocket;
    public string final_substring_to_be_searched = "result";
    //public string partial_substring_to_be_searched = "partial";
    public int final_substring_to_be_searched_length = 0;
    public int partial_substring_to_be_searched_length = 0;
    public VoiceVisualizationProcessor VPVocal;
    ActionSpaceHandler action_space_handler;
    public string current_text = "";


    public static VoiceVisualizationProcessor _VPVocal;
    public static string message;
    // TODO: 
    // - integrate get_current_command -> in render loop
    // - reset after 2/? seconds??

    public void SetVoiceVisProcessor()
    {
        _VPVocal = VPVocal;
    }

    private void Awake()
    {
        //websocket = GameObject.Find("WebsocketObject").GetComponent<MicrophoneStreamingBehavior>(); -> GET VOICE HANDLER HERE!
        final_substring_to_be_searched_length = final_substring_to_be_searched.Length;
        //partial_substring_to_be_searched_length = partial_substring_to_be_searched.Length;

        SetVoiceVisProcessor();
        action_space_handler = new ActionSpaceHandler(VPVocal);
    }

    // Start is called before the first frame update
    async void Start()
    {
        // solution found here: https://developers.deepgram.com/blog/2022/03/deepgram-unity-tutorial/
        websocket = new WebSocket("ws://localhost:2700/ws"); // ws://localhost:2700

        websocket.OnOpen += () =>
        {
            Debug.Log("CONNECTED to service_speech_to_text!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection to service_speech_to_text CLOSED!");
        };

        websocket.OnMessage += (bytes) =>
        {
            // check if bytes contain a result string or are empty
            if (bytes.Length > 6)
            {
                // getting the message as a string
                message = System.Text.Encoding.UTF8.GetString(bytes);
                Debug.Log("RESULT: " + message);
                //StartCoroutine(send_post_request(message)); 
                StartCoroutine(send_get_request(message));
            }

            //Debug.Log("MESSAGE RECEIVED: " + message);
            // ENUM HANDLER, implement as fire and forget task for speed: https://dev.to/garryxiao/c-fire-and-forget-method-3ed

            // check if message contains partial -> then do partial handler (= call function with different params), solution found here: https://stackoverflow.com/questions/3519539/how-to-check-if-a-string-contains-any-of-some-strings
            //bool partial_count = message.Contains("partial");
            // check if message contains result -> then do result handler (= call function with different params) 
            //bool result_count = message.Contains("result");
            // CHECK IF PARTIAL OR RESULT 
            /*           if(partial_count)
                        {
                            // CURRENT TEXT: get the current text
                            current_text = message.Substring(17); // take text after { partial: -> from here
                            current_text = current_text.Replace("\"", string.Empty);
                            current_text = current_text.Replace("}", string.Empty);
                            //Debug.Log(message.Length); = where to start = after character 20?
                            send_command(current_text, false, LocomotionState.Idle, TeleportObject.Idle, WalkingSpeed.Idle);

                            // APPLY FILTERS, use String.Contains() not Regex, due to performance, as reported here: https://medium.com/theburningmonk-com/performance-test-string-contains-vs-string-indexof-vs-regex-ismatch-724aa0133bd
                            // direction commands
                            bool forward_count = message.Contains("vorwärts");
                            bool backward_count = message.Contains("rückwärts");
                            bool left_count = message.Contains("links");
                            bool right_count = message.Contains("rechts");
                            bool stop_count = message.Contains("stop");
                            // teleport/move commands
                            bool move_count = message.Contains("spring");
                            // object recognition
                            bool kitchen_count = message.Contains("küche");
                            bool table_count = message.Contains("tisch");
                            bool sofa_count = message.Contains("sofa");
                            bool chair_count = message.Contains("stuhl");
                            bool bed_count = message.Contains("bett");
                            bool wardrobe_count = message.Contains("schrank");
                            bool mirror_count = message.Contains("spiegel");
                            bool lamp_count = message.Contains("lampe");
                            bool vase_count = message.Contains("vase");
                            bool sink_count = message.Contains("waschbecken");
                            bool armchair_count = message.Contains("sessel");

                            // check direction commands
                            if(forward_count)
                            {
                                current_locomotion_state = LocomotionState.WalkingForward;
                            }
                            else if (backward_count)
                            {
                                current_locomotion_state = LocomotionState.WalkingBackward;
                            }
                            else if (left_count)
                            {
                                current_locomotion_state = LocomotionState.WalkingLeft;
                            }
                            else if(right_count)
                            {
                                current_locomotion_state = LocomotionState.WalkingRight;
                            }
                            else if(stop_count)
                            {
                                current_locomotion_state = LocomotionState.Stop;
                            } 
                            // check teleport/move command
                            else if(move_count)
                            {
                                current_locomotion_state = LocomotionState.TeleportToObject;
                            }

                        }
                        else if(result_count)
                        {
                            //Debug.Log("RESULT");
                            //Debug.Log(message);
                            // example message looks like this: {"result": " Wow, such a good description."}
                            // filter out the text that has been said after "result":
                            string result_text = message.Substring(10); // take text after { result: -> from here
                            // replace the " with nothing
                            result_text = result_text.Replace("\"", string.Empty);
                            // replace the } with nothing
                            result_text = result_text.Replace("}", string.Empty);
                            // trim the string
                            result_text = result_text.Trim();
                            Debug.Log("RESULT: " + result_text);

                            // send result_text to service_language_assistant which will handle the task mapping -> start coroutine for asynchronous post request
                            StartCoroutine(send_post_request(result_text));

                            // find complete text that has been said, solution found here: https://stackoverflow.com/questions/14998595/need-to-get-a-string-after-a-word-in-a-string-in-c-sharp
            /*                 int idx = message.IndexOf(final_substring_to_be_searched);
                            if(idx != -1) 
                            {
                                current_text = message.Substring(idx + final_substring_to_be_searched_length);
                                // remove garbage in string
                                current_text = current_text.Replace("\"", string.Empty);
                                current_text = current_text.Replace(":", string.Empty);
                                current_text = current_text.Replace("}", string.Empty);
                                // send final command
                                send_command(current_text, true, current_locomotion_state, current_teleport_object, current_walking_speed);
                                // reset states
                                current_locomotion_state = LocomotionState.Idle;
                                current_teleport_object = TeleportObject.Idle;
                                current_walking_speed = WalkingSpeed.Idle;
                            } */
            /* } */
        };

        await websocket.Connect();

    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    public async void ProcessAudio(byte[] audio)
    {
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.Send(audio);
        }
    }

    //public void send_command(string current_text, bool is_result, LocomotionState current_locomotion_state, TeleportObject current_teleport_object, WalkingSpeed current_walking_speed)
    //{
    //    //Debug.Log(current_text);
    //    if (is_result)
    //    {
    //        // call functions from here
    //        Debug.Log(current_text);
    //    }
    //    else
    //    {
    //        // update text
    //    }

    //}

    // LEGACY POST REQUEST VARIANT -> we go for GET because it is faster
    /*     IEnumerator send_post_request(string result_text)
        {
            // alternative solution: https://stackoverflow.com/questions/46003824/sending-http-requests-in-c-sharp-with-unity
            WWWForm form = new WWWForm();
            form.AddField("text", result_text);

            UnityWebRequest www = UnityWebRequest.Post("http://localhost:2702/perceive_text", form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) //   if (www.isNetworkError || www.isHttpError) -> works for unity version 2019
            {
                Debug.Log("POST send failed!");
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("POST send successfully!");
                string output_string = www.downloadHandler.text;
                Debug.Log(output_string);
                // parse json_result
                // get function name and arguments
                (string functionName, string[] arguments) = action_space_handler.ParseOutputString(output_string);
                // call function by name with arguments
                action_space_handler.CallFunctionByName(functionName, arguments);
            }
            // prevent memory leak
            www.Dispose();
        } */

    // LEGACY PARSE OUTPUT VARIANT FOR STRING FORMAT: Action: Move Number: 1 Parameter: 0.5
    /*     public (string, string[]) ParseOutputString(string output_string)
        {
            // OUTPUT STRING LOOKS LIKE THIS: method: method_name, params: ['param1', 'param2', 'param3']
            // remove " from string
            output_string = output_string.Replace("\"", string.Empty);
            Debug.Log(output_string);

            string[] parts = output_string.Split(' '); // split string by space, string looks like this: Action: move Number: 1 Direction: Forward or: action: move number: 1 direction: forward

            if (parts.Length < 3 || parts.Length % 2 != 0) // if we have less than 3 parts or an uneven number of parts, we have an invalid format
            {
                Console.WriteLine("Invalid JSON result format");
                return (string.Empty, new string[0]);
            } 

            string functionName = parts[1].Trim().ToLower();
            string[] arguments = new string[(parts.Length - 2) / 2];

            for (int i = 2, j = 0; i < parts.Length; i += 2, j++)
            {
                string argumentValue = parts[i + 1].Trim();
                arguments[j] = argumentValue;
            }

            return (functionName, arguments); 
        } */

    IEnumerator send_get_request(string result_text)
    {
        string url = "http://localhost:2702/perceive_text?input_string=" + UnityWebRequest.EscapeURL(result_text);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) //-> works for Unity version 2019
        {
            Debug.Log("GET request failed!");
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("GET request successful!");
            string output_string = www.downloadHandler.text;
            Debug.Log(output_string);
            // parse json_result 
            // get function name and arguments
            (string functionName, List<string> arguments) = action_space_handler.ParseOutputString(output_string);
            // call function by name with arguments
            action_space_handler.CallFunctionByName(functionName, arguments);
        }
        // prevent memory leak
        www.Dispose();
    }

    public IEnumerator set_locomotion_mode(string new_locomotion_mode)
    {
        string url = "http://localhost:2702/set_locomotion_mode?locomotion_mode=" + UnityWebRequest.EscapeURL(new_locomotion_mode);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        //if (www.result != UnityWebRequest.Result.Success) 
        if (www.isNetworkError || www.isHttpError) //-> works for Unity version 2019
        {
            Debug.Log("GET request failed!");
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("GET request successful for setting locomotion mode!");
            string output_string = www.downloadHandler.text;
            Debug.Log(output_string);
        }
        // prevent memory leak
        www.Dispose();
    }
}


// ACTION SPACE HANDLER 
public class ActionSpaceHandler : MonoBehaviour
{
    public VoiceVisualizationProcessor VPVocal;
    public ActionSpaceHandler(VoiceVisualizationProcessor input)
    {

        VPVocal = input;
    }
    public void CallFunctionByName(string functionName, List<string> arguments)
    {
        // call function by name with parameters
        Type thisType = typeof(ActionSpaceHandler);
        MethodInfo method = thisType.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (method != null)
        {
            method.Invoke(this, arguments.ToArray());
        }
        else
        {
            Debug.LogError($"Function '{functionName}' does not exist!");
        }
    }

    public (string, List<string>) ParseOutputString(string output_string)
    {
        // OUTPUT STRING LOOKS LIKE THIS: method: method_name, params: ['param1', 'param2', 'param3']

        // split the string into two parts by the first comma (,) in the string seen from the left
        int index = output_string.IndexOf(',');
        // GET FUNCTION NAME
        string functionName = output_string.Substring(output_string.IndexOf(':') + 1, index - output_string.IndexOf(':') - 1).Trim().ToLower();
        // GET ARGUMENTS
        string paramsSubstring = output_string.Substring(index + 1).Split(':')[1].Trim();

        // remove " from string
        //output_string = output_string.Replace("\"", string.Empty);
        //Debug.Log(output_string);

        // split the string into two parts by the first comma (,) in the string seen from the left
        //int index = output_string.IndexOf(',');
        // GET FUNCTION NAME
        //string functionSubstring = output_string.Substring(0, index);
        // the function substring looks like this: method: method_name -> get the method name
        //string functionName = functionSubstring.Split(':')[1].Trim().ToLower();
        //Debug.Log(functionName);

        // GET ARGUMENTS
        //string paramsSubstring = output_string.Substring(index + 1);
        //Debug.Log(paramsSubstring);
        // the params substring looks like this: params: ['param1', 'param2', 'param3'] -> get the params
        //paramsSubstring = paramsSubstring.Split(':')[1].Trim();
        //Debug.Log(paramsSubstring);

        // get all parameters out of the paramsSubstring by matching all strings between ' and ' or all numbers
        List<string> arguments = new List<string>();
        MatchCollection matches = Regex.Matches(paramsSubstring, @"'([^'\\]*(?:\\.[^'\\]*)*)'|(-?\d+(?:\.\d+)?)\b");

        foreach (Match match in matches)
        {
            if (match.Groups[1].Success)
            {
                arguments.Add(match.Groups[1].Value);
            }
            else if (match.Groups[2].Success)
            {
                arguments.Add(match.Groups[2].Value);
            }
        }

        return (functionName, arguments);
    }


    // ---------------------------------------------------------------- 
    // LIST OF FUNCTIONS TO BE CALLED



    public void rotate_x(string rotation_angle)
    {
        VPVocal.RotateX(float.Parse(rotation_angle, CultureInfo.InvariantCulture));
    }

    public void rotate_y(string rotation_angle)
    {
        VPVocal.RotateY(float.Parse(rotation_angle, CultureInfo.InvariantCulture));
    }

    public void rotate_z(string rotation_angle)
    {
        VPVocal.RotateZ(float.Parse(rotation_angle, CultureInfo.InvariantCulture));

    }

    public void rotate_x_continuously()
    {
        VPVocal.RotateXContinuously();

    }

    public void rotate_y_continuously()
    {
        VPVocal.RotateYContinuously();

    }

    public void rotate_z_continuously()
    {
        VPVocal.RotateZContinuously();

    }

    public void adjust_rotation_speed(string speed_modifier)
    {
        VPVocal.AdjustRotationSpeed(Helper.GetSpeedModifierFromString(speed_modifier));

    }

    public void stop_rotation()
    {
        VPVocal.StopRotation();

    }

    public void set_view(string view)
    {
        VPVocal.SetView(Helper.GetCanonicalViewDirectionFromString(view));

    }

    public void reset_object()
    {
        VPVocal.Reset();

    }

    public void switch_visualization(string vis)
    {
        //VPVocal.SwitchVisualization(Helper.GetVisualisationTypeFromString(vis));
        VPVocal.SwitchVisualization(VisualisationType.Heatmap);

    }

    public void enable_isolines(string enable)
    {
        VPVocal.EnableIsolines(bool.Parse(enable));

    }

    public void set_heatmap_range(string range_value)
    {
        VPVocal.SetHeatmapRange(float.Parse(range_value, CultureInfo.InvariantCulture));

    }

    public void set_main_color(string color)
    {
        VPVocal.SetMainColor(Helper.GetColorsFromString(color));

    }

    public void set_secondary_color(string color)
    {
        VPVocal.SetSecondaryColor(Helper.GetColorsFromString(color));

    }

    public void show_values_at(string val)
    {
        VPVocal.ShowValuesAt(Helper.GetMinMaxValueFromString(val));

    }

    public void switch_scalar_field(string data_vis)
    {
        VPVocal.SwitchScalarField(Helper.GetScalarFieldDataFromString(data_vis));

    }

    public void show_split_of_data(string val, string color, string percentage)
    {
        VPVocal.ShowSplitOfData(Helper.GetMinMaxValueFromString(val), Helper.GetColorsFromString(color), float.Parse(percentage));

    }

    public void change_time_step(string step)
    {
        VPVocal.ChangeTimeStep(int.Parse(step));

    }



    public void do_nothing()
    {

    }
    // ---------------------------------------------------------------- 
}