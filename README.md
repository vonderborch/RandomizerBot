# RandomizerBot
A simple Discord Bot that can be used to randomize lists, dice rolls, etc.

# Available Commands
Command                         | Description                                                     | Example Command Format                                                                      
------------------------------- | --------------------------------------------------------------- | --------------------------------------------------------------------------------------------
!rb_help                        | Displays a general help message to the user                     | !rb_help [command] [show_parameters] [as_table]                                             
!rb_bot_info                    | Displays some basic info about the Bot                          | !rb_bot_info                                                                                
!rb_bot_version                 | Displays the current Bot Version                                | !rb_bot_version                                                                             
!rb_report_issue                | Provides a link to report an issue                              | !rb_report_issue                                                                            
!rb_changelog                   | Prints out the bot's changelog                                  | !rb_changelog [git_link] [local_changelog]                                                  
!rb_dev_todo                    | Prints out the bot's development todo list                      | !rb_dev_todo [git_link] [local_changelog]                                                   
!rb_flip_coin                   | Flips a coin and returns heads or tails depending on the result | !rb_flip_coin [times] [show_individual_results]                                             
!rb_roll_die                    | Rolls a Die with variable sides dice for the user               | !rb_roll_die [faces] [times] [show_individual_results]                                      
!rb_roll_d4                     | Rolls a D4 dice for the user                                    | !rb_roll_d4 [times] [show_individual_results]                                               
!rb_roll_d6                     | Rolls a D6 dice for the user                                    | !rb_roll_d6 [times] [show_individual_results]                                               
!rb_roll_d8                     | Rolls a D8 dice for the user                                    | !rb_roll_d8 [times] [show_individual_results]                                               
!rb_roll_d10                    | Rolls a D10 dice for the user                                   | !rb_roll_d10 [times] [show_individual_results]                                              
!rb_roll_d12                    | Rolls a D12 dice for the user                                   | !rb_roll_d12 [times] [show_individual_results]                                              
!rb_roll_d20                    | Rolls a D20 dice for the user                                   | !rb_roll_d20 [times] [show_individual_results]                                              
!rb_random_list                 | Randomizes a comma-separated list of items.                     | !rb_random_list <items> [show_first_item_only] [show_original_list]                         
!rb_itemlist_view_all           | Views all available item lists                                  | !rb_itemlist_view_all [show_personal_lists] [show_server_lists]                             
!rb_itemlist_create             | Creates a new list                                              | !rb_itemlist_create <name> [is_personal_list]                                               
!rb_itemlist_delete             | Deletes a list                                                  | !rb_itemlist_delete <name> [is_personal_list]                                               
!rb_itemlist_change_list_name   | Changes the name of an item list                                | !rb_itemlist_change_list_name <name> <new_name> [is_personal_list]                          
!rb_itemlist_view_items         | Views all items in an item list                                 | !rb_itemlist_view_items <name> [is_personal_list]                                           
!rb_itemlist_add_item           | Adds an item to an item list                                    | !rb_itemlist_add_item <name> <new_item> [is_personal_list] [weight]                         
!rb_itemlist_delete_item        | Deletes an item in an item list                                 | !rb_itemlist_delete_item <name> <item_to_delete> [is_personal_list]                         
!rb_itemlist_change_item_name   | Changes the name of an item in an item list                     | !rb_itemlist_change_item_name <name> <item> <new_name> [is_personal_list]                   
!rb_itemlist_change_item_weight | Changes the weight of an item in an item list                   | !rb_itemlist_change_item_weight <name> <item> <new_weight> [is_personal_list]               
!rb_itemlist_enable_item        | Enables an item in an item list for randomization               | !rb_itemlist_enable_item <name> <item> [is_personal_list]                                   
!rb_itemlist_disables_item      | Disables an item in an item list for randomization              | !rb_itemlist_disables_item <name> <item> [is_personal_list]                                 
!rb_itemlist_toggle_item        | Toggles an item in an item list for randomization               | !rb_itemlist_toggle_item <name> <item> [is_personal_list]                                   
!rb_itemlist_enable_all_items   | Enables all in an item list for randomization                   | !rb_itemlist_enable_all_items <name> [is_personal_list]                                     
!rb_itemlist_disable_all_items  | Disables all in an item list for randomization                  | !rb_itemlist_disable_all_items <name> [is_personal_list]                                    
!rb_itemlist_toggle_all_items   | Toggles all in an item list for randomization                   | !rb_itemlist_toggle_all_items <name> [is_personal_list]                                     
!rb_itemlist_randomize          | Randomizes an item list                                         | !rb_itemlist_randomize <name> [is_personal_list] [show_original_list] [show_first_item_only]
