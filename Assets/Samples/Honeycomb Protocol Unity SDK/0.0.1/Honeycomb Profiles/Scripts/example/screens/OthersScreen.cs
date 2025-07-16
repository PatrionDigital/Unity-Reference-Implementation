using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HplEdgeClient.Params;
using HplEdgeClient.Inputs;
using HplEdgeClient.Helpers;
using System;
using HplEdgeClient.Enums;
using System.Text;
using Solana.Unity.Wallet.Utilities;
using Solana.Unity.Wallet;
using Solana.Unity.Programs;
using Solana.Unity.Metaplex.NFT.Library;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Metaplex.Utilities;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Rpc.Types;
using System.Linq;


namespace Solana.Unity.SDK.Example
{
    public class OtherScreen : SimpleScreen
    {

        [SerializeField]
        private TMP_Text resultText;

        [SerializeField]
        private TMP_Dropdown functionalitySelector;

        [SerializeField]
        private Button runBtn;


        [SerializeField]
        private Button back_btn;


        private string projectAddress;
        private string adminPublicKey;
        private string delegatePublicAddress;
        private string txPayerPublicKey;
        private string payerPublicKey;
        private string newDriverAddress;
        private string delegatePublicKey;
        private string authorityPublicKey;

        private string profileAddress = "";

        private string resourceAddress = "";

        private string delegateAuthority = "";

        private string collection;

        private string creatorPublicKey;

        private string stakingPoolAddress = "";

        private string multipliersAddress = "";

        private string characterModelAddress = "";

        private string characterAddress = "";

        private int UserID = 0;

        private string nftMinAddress = "";

        private string missionPoolAddress = "";

        private string missionAddress = "";

        //list of strings name trees
        private List<string> trees = new List<string>();






        public void Start()
        {
            authorityPublicKey = Web3.Wallet.Account.PublicKey.ToString();
            adminPublicKey = Web3.Wallet.Account.PublicKey.ToString();
            delegatePublicAddress = Web3.Wallet.Account.PublicKey.ToString();
            txPayerPublicKey = Web3.Wallet.Account.PublicKey.ToString();
            payerPublicKey = Web3.Wallet.Account.PublicKey.ToString();
            newDriverAddress = Web3.Wallet.Account.PublicKey.ToString();
            delegatePublicKey = Web3.Wallet.Account.PublicKey.ToString();
            profileAddress = Web3.Wallet.Account.PublicKey.ToString();
            delegateAuthority = Web3.Wallet.Account.PublicKey.ToString();
            collection = Web3.Wallet.Account.PublicKey.ToString();
            creatorPublicKey = Web3.Wallet.Account.PublicKey.ToString();
            projectAddress = "";



            back_btn.onClick.AddListener(() =>
            {
                // StateManager.Instance.SaveCreateProfileInfo("username", usernameInputField.text);
                // StateManager.Instance.SaveCreateProfileInfo("name", nameInputField.text);
                // StateManager.Instance.SaveCreateProfileInfo("bio", bioInputField.text);
                manager.ShowScreen(this, "wallet_screen");
            });

            runBtn.onClick.AddListener(
                () =>
                {
                    //console value of functionalitySelector
                    Debug.Log(functionalitySelector.value);
                    //switch case for the value of the dropdown
                    switch (functionalitySelector.value)
                    {
                        case 0:
                            CreateProject();
                            break;
                        case 1:
                            ChangeProjectDriver();
                            break;
                        case 2:
                            CreateDelegateAuthoriy();
                            break;
                        case 3:
                            ModifyDelegateAuthority();
                            break;
                        case 4:
                            CreatBadge();
                            break;
                        case 5:
                            UpdateBadge();
                            break;
                        case 6:
                            AssignBadgeToProfile();
                            break;
                        case 7:
                            CreateNewUser();
                            break;
                        case 8:
                            UpdateUser();
                            break;
                        case 9:
                            CreateProfile();
                            break;
                        case 10:
                            UpdateProfile();
                            break;
                        case 11:
                            CreateUserWithProfile();
                            break;
                        case 12:
                            CreateCharacterModel();
                            break;
                        case 13:
                            WrapAssets();
                            break;
                        case 14:
                            UnwrapAssets();
                            break;
                        case 15:
                            CreateUnCompressedResource();
                            break;
                        case 16:
                            CreateCompressedResource();
                            break;
                        case 17:
                            MintResource();
                            break;
                        case 18:
                            BurnResource();
                            break;
                        case 19:
                            CreateStakingPool();
                            break;
                        case 20:
                            UpdateStakingPool();
                            break;
                        case 21:
                            CreateMultipliers();
                            break;
                        case 22:
                            AddMultipliers();
                            break;
                        case 23:
                            StakeCharacters();
                            break;
                        case 24:
                            ClaimStakingRewards();
                            break;
                        case 25:
                            CreateMissionPool();
                            break;
                        case 26:
                            CreateMission();
                            break;
                        case 27:
                            SendCharactersOnMission();
                            break;
                        case 28:
                            RecallCharactersFromMission();
                            break;
                        case 29:
                            CreateCharacterModelAssembled();
                            break;

                        default:
                            break;
                    }

                }
            );
        }


        private async void CreateNFTMint()
        {

            // Mint and ATA
            var mint = new Account();
            var associatedTokenAccount = AssociatedTokenAccountProgram
                .DeriveAssociatedTokenAccount(Web3.Account, mint.PublicKey);
            // Define the metadata
            var metadata = new Metadata()
            {
                name = "Solana Unity SDK NFT",
                symbol = "MGCK",
                uri = "https://y5fi7acw5f5r4gu6ixcsnxs6bhceujz4ijihcebjly3zv3lcoqkq.arweave.net/x0qPgFbpex4ankXFJt5eCcRKJzxCUHEQKV43mu1idBU",
                sellerFeeBasisPoints = 0,
                creators = new List<Creator> { new(Web3.Account.PublicKey, 100, true) }
            };

            // Prepare the transaction
            var blockHash = await Web3.Rpc.GetLatestBlockHashAsync();
            var minimumRent = await Web3.Rpc.GetMinimumBalanceForRentExemptionAsync(TokenProgram.MintAccountDataSize);
            var transaction = new TransactionBuilder()
                .SetRecentBlockHash(blockHash.Result.Value.Blockhash)
                .SetFeePayer(Web3.Account)
                .AddInstruction(
                    SystemProgram.CreateAccount(
                        Web3.Account,
                        mint.PublicKey,
                        minimumRent.Result,
                        TokenProgram.MintAccountDataSize,
                        TokenProgram.ProgramIdKey))
                .AddInstruction(
                    TokenProgram.InitializeMint(
                        mint.PublicKey,
                        0,
                        Web3.Account,
                        Web3.Account))
                .AddInstruction(
                    AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
                        Web3.Account,
                        Web3.Account,
                        mint.PublicKey))
                .AddInstruction(
                    TokenProgram.MintTo(
                        mint.PublicKey,
                        associatedTokenAccount,
                        1,
                        Web3.Account))
                .AddInstruction(MetadataProgram.CreateMetadataAccount(
                    PDALookup.FindMetadataPDA(mint),
                    mint.PublicKey,
                    Web3.Account,
                    Web3.Account,
                    Web3.Account.PublicKey,
                    metadata,
                    TokenStandard.NonFungible,
                    true,
                    true,
                    null,
                    metadataVersion: MetadataVersion.V3))
                .AddInstruction(MetadataProgram.CreateMasterEdition(
                        maxSupply: null,
                        masterEditionKey: PDALookup.FindMasterEditionPDA(mint),
                        mintKey: mint,
                        updateAuthorityKey: Web3.Account,
                        mintAuthority: Web3.Account,
                        payer: Web3.Account,
                        metadataKey: PDALookup.FindMetadataPDA(mint),
                        version: CreateMasterEditionVersion.V3
                    )
                );
            var tx = Transaction.Deserialize(transaction.Build(new List<Account> { Web3.Account, mint }));

            // Sign and Send the transaction
            var res = await Web3.Wallet.SignAndSendTransaction(tx);

            // Show Confirmation
            if (res?.Result != null)
            {
                await Web3.Rpc.ConfirmTransaction(res.Result, Commitment.Confirmed);
                Debug.Log("Minting succeeded, see transaction at https://explorer.solana.com/tx/"
                          + res.Result + "?cluster=" + Web3.Wallet.RpcCluster.ToString().ToLower());

                nftMinAddress = mint.PublicKey.ToString();
            }


        }

        public async void getandSaveProfileandUserID()
        {
            if (projectAddress == "")
            {
                Debug.LogError("Please create a project first");
                return;

            }
            var honeycombUser = await StateManager.Instance.Client.FindUsers(new
            FindUsersParams
            {
                Wallets = new List<string> { Web3.Wallet.Account.PublicKey.ToString() }
            }
            );
            if (honeycombUser.User.Count > 0)
            {
                Debug.Log(honeycombUser.User[0].Id + " User ID Saved");
                UserID = honeycombUser.User[0].Id;

                var profileResponse = await StateManager.Instance.Client.FindProfiles(new FindProfilesParams
                {
                    Projects = new List<string> { projectAddress.ToString() },
                    UserIds = new List<int> { honeycombUser.User[0].Id }
                });

                if (profileResponse.Profile.Count > 0)
                {
                    Debug.Log(profileResponse.Profile[0].Address + " Profile Address Saved");
                    profileAddress = profileResponse.Profile[0].Address;
                }


            }
        }




        public async void CreateProject()
        {


            try
            {

                if (projectAddress != "")
                {
                    resultText.color = Color.red;
                    resultText.text = "Project and Profile Tree already exists";
                    //console project address
                    Debug.Log("Project already exists with ID: " + projectAddress);
                    Debug.LogError("Project already exists");
                    return;
                }
                var createProjectParams = new CreateCreateProjectTransactionParams
                {
                    Name = "Test Project", // Name of the project
                    Authority = Web3.Wallet.Account.PublicKey.ToString(), // Public key of the project authority, who has complete control over the project
                    Payer = Web3.Wallet.Account.PublicKey.ToString(), // Optional: public key of the payer, who pays the transaction fees
                                                                      // ProfileDataConfig = new ProfileDataConfigInput
                                                                      // {
                                                                      //     Achievements = new List<string> // Specify an array of achievements for user profiles
                                                                      //     {
                                                                      //         "Pioneer",
                                                                      //     },

                    //     CustomDataFields = new List<string> // Specify an array of custom data fields for user profiles
                    //     {
                    //         "NFTs owned"
                    //     }
                    // }        
                };

                // Create a project transaction
                var tx = await StateManager.Instance.Client.CreateCreateProjectTransaction(createProjectParams);
                // Extract project address and transaction response from the result

                var txResponse = new TransactionHelper(tx.CreateCreateProjectTransaction.Tx); // This is the transaction response, which you'll need to sign and send
                var txHelper = await txResponse.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(txHelper.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    projectAddress = tx.CreateCreateProjectTransaction.Project; // This is the project address once created
                    Debug.Log("Project created successfully");
                    Console.WriteLine("Project created successfully with ID: " + projectAddress);
                    Debug.Log(res.ToString());

                    resultText.text = "Project created successfully with ID: " + projectAddress;
                    CreateProfileTree();
                    getandSaveProfileandUserID();
                    CreateNFTMint();
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to create Project";
                    Debug.LogError("Failed to create Project");
                }

            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }

        }

        public async void ChangeProjectDriver()
        {
            if (projectAddress == "")
            {
                CreateProject();
            }

            try
            {

                var changeDriverParams = new CreateChangeProjectDriverTransactionParams
                {
                    Authority = adminPublicKey, // Provide the project authority's public key
                    Project = projectAddress.ToString(),    // The project's public key
                    Driver = newDriverAddress.ToString(),   // The new driver's public key
                    Payer = payerPublicKey.ToString()       // Optional: the transaction payer's public key
                };

                // Create a change project driver transaction
                var tx = await StateManager.Instance.Client.CreateChangeProjectDriverTransaction(changeDriverParams);
                var txHelper = new TransactionHelper(tx.CreateChangeProjectDriverTransaction);
                var txResponse = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(txResponse.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("Driver changed successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Driver changed successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to change driver";
                    Debug.LogError("Failed to change driver");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }

        }

        public async void CreateDelegateAuthoriy()
        {

            if (projectAddress == "")
            {
                CreateProject();
            }
            try
            {

                var createDelegateAuthorityParams = new CreateCreateDelegateAuthorityTransactionParams
                {
                    Authority = adminPublicKey.ToString(), // Provide the project authority's public key
                    Delegate = delegatePublicAddress.ToString(), // The delegate's public address
                    Project = projectAddress.ToString(), // The project's public key
                    Payer = txPayerPublicKey.ToString(), // Optional: the transaction payer's public key
                    ServiceDelegations = new ServiceDelegationInput
                    {

                        HiveControl = new List<ServiceDelegationHiveControl> // Specify the service name, e.g., HiveControl
                        {
                            new ServiceDelegationHiveControl
                            {
                                // Each service's permissions enum can be imported from using HplEdgeClient.Enums;
                                Permission=  HiveControlPermissionInput.ManageProjectDriver // Permission enum for the service
                                // In some cases, an index will also be required in this object, for example: index: 0
                            },
                        },


                    }
                };

                // Create a delegate authority transaction
                var tx = await StateManager.Instance.Client.CreateCreateDelegateAuthorityTransaction(createDelegateAuthorityParams); // txResponse now contains the transaction response, which you'll need to sign and send
                var txHelper = new TransactionHelper(tx.CreateCreateDelegateAuthorityTransaction);
                var txResponse = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(txResponse.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("Delegate authority created successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Delegate authority created successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to create delegate authority";
                    Debug.LogError("Failed to create delegate authority");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public async void ModifyDelegateAuthority()
        {
            try
            {
                if (projectAddress == "")
                {
                    CreateProject();
                }

                // Create parameters for modifying the delegation transaction
                var createModifyDelegationParams = new CreateModifyDelegationTransactionParams
                {
                    Authority = adminPublicKey.ToString(), // Provide the project authority's public key
                    Project = projectAddress.ToString(),    // The project's public key
                    Delegate = delegatePublicKey.ToString(), // The delegate's public key
                    ModifyDelegation = new ModifyDelegationInput
                    {
                        Delegation = new ModifyServiceDelegationInput
                        {
                            HiveControl = new ServiceDelegationHiveControl
                            {
                                Permission = HiveControlPermissionInput.ManageCriterias // Set the permission for the service
                            }
                        }
                    },
                    Payer = payerPublicKey.ToString(), // Optional: the transaction payer's public key

                };

                // Create a modify delegation transaction using the specified parameters
                var tx = await StateManager.Instance.Client.CreateModifyDelegationTransaction(createModifyDelegationParams);
                var txHelper = new TransactionHelper(tx.CreateModifyDelegationTransaction);
                var txResponse = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(txResponse.ToSendParams(new SendTransactionBundlesOptions
                {
                    SkipPreflight = true
                }));
                if (res.SendBulkTransactions[0].Error == null)
                {
                    resultText.color = Color.green;
                    Debug.Log("Delegate authority modified successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Delegate authority modified successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to modify delegate authority";
                    Debug.LogError("Failed to modify delegate authority");
                }

            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }


        }

        private async void CreatBadge()
        {
            if (projectAddress == "")
            {
                CreateProject();
            }
            try
            {

                // Create parameters for the create badge criteria transaction
                var createBadgeCriteriaParams = new CreateInitializeBadgeCriteriaTransactionParams
                {
                    Args = new CreateBadgeCriteriaInput
                    {
                        Authority = authorityPublicKey,    // Project authority public key
                        ProjectAddress = projectAddress,    // Project public key
                        Payer = payerPublicKey,             // Optional transaction payer public key
                        BadgeIndex = 0,                     // Badge index as an integer, used to identify the badge
                        Condition = BadgesCondition.Public,  // Badge condition, only Public is available for now
                        StartTime = 0,                      // Optional start time, UNIX timestamp
                        EndTime = 0,                        // Optional end time, UNIX timestamp
                    }
                };

                // Create a create badge criteria transaction
                var txResponse = await StateManager.Instance.Client.CreateInitializeBadgeCriteriaTransaction(createBadgeCriteriaParams);

                // Extract relevant information from the transaction response
                var blockhash = txResponse.CreateInitializeBadgeCriteriaTransaction.Blockhash; // Blockhash of the transaction
                var lastValidBlockHeight = txResponse.CreateInitializeBadgeCriteriaTransaction.LastValidBlockHeight; // Last valid block height
                var transaction = txResponse.CreateInitializeBadgeCriteriaTransaction; // The transaction object

                // Sign the transaction
                var txHelper = new TransactionHelper(transaction);
                var signedTx = await txHelper.Sign();

                // Send the transaction
                var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("Badge criteria created successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Badge criteria created successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to create badge criteria";
                    Debug.LogError("Failed to create badge criteria");

                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }


        private async void UpdateBadge()
        {
            try
            {
                if (projectAddress == "")
                {
                    CreateProject();
                }
                // Create parameters for the update badge criteria transaction
                var updateBadgeCriteriaParams = new CreateUpdateBadgeCriteriaTransactionParams
                {
                    Args = new UpdateBadgeCriteriaInput
                    {
                        Authority = authorityPublicKey,    // Project authority public key
                        ProjectAddress = projectAddress,    // Project public key
                        Payer = payerPublicKey,             // Optional transaction payer public key
                        CriteriaIndex = 0,                     // Badge index as an integer, used to identify the badge
                        Condition = BadgesCondition.Public,  // Badge condition, only Public is available for now
                        StartTime = 0,                      // Optional start time, UNIX timestamp
                        EndTime = 0,                        // Optional end time, UNIX timestamp
                    }
                };

                // Create a update badge criteria transaction
                var txResponse = await StateManager.Instance.Client.CreateUpdateBadgeCriteriaTransaction(updateBadgeCriteriaParams);

                // Extract relevant information from the transaction response
                var blockhash = txResponse.CreateUpdateBadgeCriteriaTransaction.Blockhash; // Blockhash of the transaction
                var lastValidBlockHeight = txResponse.CreateUpdateBadgeCriteriaTransaction.LastValidBlockHeight; // Last valid block height
                var transaction = txResponse.CreateUpdateBadgeCriteriaTransaction; // The transaction object

                // Sign the transaction
                var txHelper = new TransactionHelper(transaction);
                var signedTx = await txHelper.Sign();

                // Send the transaction
                var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("Badge criteria updated successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Badge criteria updated successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to update badge criteria";
                    Debug.LogError("Failed to update badge criteria");

                }


            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public async void AssignBadgeToProfile()
        {
            if (projectAddress == "")
            {
                CreateProject();
            }

            if (profileAddress == "")
            {
                resultText.color = Color.red;
                resultText.text = "Please create a profile first";
                Debug.LogError("Please create a profile first");
                return;
            }


            try
            {
                // Create parameters for the claim badge criteria transaction
                var claimBadgeCriteriaParams = new CreateClaimBadgeCriteriaTransactionParams
                {
                    Args = new ClaimBadgeCriteriaInput
                    {
                        ProfileAddress = profileAddress,     // User profile public key, this profile will be assigned the badge
                        ProjectAddress = projectAddress,     // Project public key
                        Proof = BadgesCondition.Public,      // Proof of the badge, only Public is available for now
                        Payer = payerPublicKey,    // Transaction payer public key
                        CriteriaIndex = 0                    // Badge index as an integer, used to identify the badge
                    }
                };

                // Create a claim badge criteria transaction
                var txResponse = await StateManager.Instance.Client.CreateClaimBadgeCriteriaTransaction(claimBadgeCriteriaParams);

                // Extract relevant information from the transaction response
                var blockhash = txResponse.CreateClaimBadgeCriteriaTransaction.Blockhash; // Blockhash of the transaction
                var lastValidBlockHeight = txResponse.CreateClaimBadgeCriteriaTransaction.LastValidBlockHeight; // Last valid block height
                var transaction = txResponse.CreateClaimBadgeCriteriaTransaction; // The transaction object

                // Sign the transaction
                var txHelper = new TransactionHelper(transaction);
                var signedTx = await txHelper.Sign();

                // Send the transaction
                var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("Badge assigned to profile successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Badge assigned to profile successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to assign badge to profile";
                    Debug.LogError("Failed to assign badge to profile");

                }

            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }


        public async void CreateNewUser()
        {
            var userPublicKey = Web3.Wallet.Account.PublicKey;
            // Create parameters for the new user transaction
            var createNewUserParams = new CreateNewUserTransactionParams
            {
                Wallet = userPublicKey.ToString(), // User's wallet public key
                Info = new UserInfoInput // Optional, user's information
                {
                    Name = "Test User", // User's name
                    Pfp = "https://lh3.googleusercontent.com/-Jsm7S8BHy4nOzrw2f5AryUgp9Fym2buUOkkxgNplGCddTkiKBXPLRytTMXBXwGcHuRr06EvJStmkHj-9JeTfmHsnT0prHg5Mhg", // User's profile picture URL
                    Bio = "This is a test user" // User's bio
                },
                Payer = adminPublicKey.ToString() // Optional: the transaction payer's public key
            };

            // Create a new user transaction
            var txResponse = await StateManager.Instance.Client.CreateNewUserTransaction(createNewUserParams);
            var txHelper = new TransactionHelper(txResponse.CreateNewUserTransaction);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                resultText.color = Color.green;
                Debug.Log("New user created successfully");
                Debug.Log(res.ToString());
                resultText.text = "New user created successfully";
                getandSaveProfileandUserID();
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to create new user";
                Debug.LogError("Failed to create new user");
            }
        }


        public async void CreateUserWithProfile()
        {


            if (UserID != 0)
            {
                resultText.color = Color.red;
                resultText.text = "User already exists";
                Debug.LogError("User already exists, Creating Profile");
                var profileResponse = await StateManager.Instance.Client.FindProfiles(new FindProfilesParams
                {
                    Projects = new List<string> { projectAddress.ToString() },
                    UserIds = new List<int> { UserID }
                });

                if (profileResponse.Profile.Count > 0)
                {
                    Debug.Log(profileResponse.Profile[0].Address.ToString() + " Profile Address Saved");
                    profileAddress = profileResponse.Profile[0].Address.ToString();
                    resultText.color = Color.red;
                    resultText.text = "Profile already exists";
                    Debug.LogError("Profile already exists");
                    return;
                }
                else
                {
                    CreateProfile();
                }
                return;
            }
            else
            {
                var userPublicKey = Web3.Wallet.Account.PublicKey;
                // Create parameters for the new user with profile transaction
                var createNewUserWithProfileParams = new CreateNewUserWithProfileTransactionParams
                {
                    Project = projectAddress.ToString(), // Project public key
                    Wallet = userPublicKey.ToString(),   // User's wallet public key
                    Payer = adminPublicKey.ToString(),    // Transaction payer's public key
                    ProfileIdentity = "main",             // Identity of the profile to be created
                    UserInfo = new UserInfoInput // Optional: user's information
                    {
                        Name = "Honeycomb Developer", // User's name
                        Bio = "This user is created for testing purposes", // User's bio
                        Pfp = "https://lh3.googleusercontent.com/-Jsm7S8BHy4nOzrw2f5AryUgp9Fym2buUOkkxgNplGCddTkiKBXPLRytTMXBXwGcHuRr06EvJStmkHj-9JeTfmHsnT0prHg5Mhg", // User's profile picture URL
                    },

                };

                // Create a new user with profile transaction
                var txResponse = await StateManager.Instance.Client.CreateNewUserWithProfileTransaction(createNewUserWithProfileParams); // txResponse now contains the transaction response, which you'll need to sign and send
                var txHelper = new TransactionHelper(txResponse.CreateNewUserWithProfileTransaction);
                var signedTx = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    resultText.color = Color.green;
                    Debug.Log("New user with profile created successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "New user with profile created successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to create new user with profile";
                    Debug.LogError("Failed to create new user with profile");
                }
            }
        }

        public async void UpdateUser()
        {

            var authmsg = await StateManager.Instance.Client.AuthRequest(new AuthRequestParams
            {
                Wallet = Web3.Wallet.Account.PublicKey.ToString(),
            });
            var msgToSign = authmsg.AuthRequest.Message.Trim().Replace("\u200B", "");
            var messageBytes = Encoding.UTF8.GetBytes(msgToSign);
            //Debug.Log(msgToSign);
            var sign = await Web3.Wallet.SignMessage(messageBytes);
            // Debug.Log("Signature Valid: " + Web3.Wallet.Account.PublicKey.Verify(messageBytes, sign));

            //Send the signed message to the server
            var authToken = await StateManager.Instance.Client.AuthConfirm(new AuthConfirmParams
            {
                Wallet = Web3.Wallet.Account.PublicKey.ToString(),
                Signature = Encoders.Base58.EncodeData(sign)
            });

            // accessToken
            var accessToken = authToken.AuthConfirm.AccessToken;

            var updateUserTransaction = await StateManager.Instance.Client.CreateUpdateUserTransaction(new CreateUpdateUserTransactionParams
            {
                Payer = authorityPublicKey, // The public key of the user who is updating their information
                PopulateCivic = true, // Optional, set to true if you want to populate the Civic Pass information
                Wallets = new UpdateWalletInput // Optional, add or remove wallets from the user's Honeycomb Protocol account
                {
                    Add = new List<string> { "51WYx4Ke34upAk2AC2mgqovPgiYyfuzL43G9wNhSKgFH" }, // Optional, add any wallets to the user's Honeycomb Protocol account
                                                                                               // Remove = new List<string> { oldPublicKey } // Optional, remove any wallets from the user's Honeycomb Protocol account
                },
                Info = new PartialUserInfoInput // Optional, user's information
                {
                    Bio = "Updated user bio", // Optional, updated user bio
                    Name = "Honeycomb Developer", // Optional, updated name
                    Pfp = "https://lh3.googleusercontent.com/-Jsm7S8BHy4nOzrw2f5AryUgp9Fym2buUOkkxgNplGCddTkiKBXPLRytTMXBXwGcHuRr06EvJStmkHj-9JeTfmHsnT0prHg5Mhg", // Optional, updated profile picture
                }
            },
            authToken: accessToken // Required, you'll need to authenticate the user with our Edge Client and provide the resulting access token here, otherwise this operation will fail
            );

            var txResponse = updateUserTransaction.CreateUpdateUserTransaction; // This is the transaction response, you'll need to sign and send this transaction
            var txHelper = new TransactionHelper(txResponse);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                resultText.color = Color.green;
                Debug.Log("User updated successfully");
                Debug.Log(res.ToString());
                resultText.text = "User updated successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to update user";
                Debug.LogError("Failed to update user");
            }
        }


        public async void CreateProfileTree()
        {
            // Create parameters for the create profiles tree transaction
            var createProfilesTreeParams = new CreateCreateProfilesTreeTransactionParams
            {
                Payer = adminPublicKey.ToString(), // Public key of the payer
                Project = projectAddress.ToString(), // Project public key
                TreeConfig = new TreeSetupConfig // Configuration for the profiles tree
                {
                    Basic = new BasicTreeConfig // Basic configuration for the profiles tree
                    {
                        NumAssets = 10000 // The desired number of profiles this tree will be able to store
                    }
                    // Uncomment the following config if you want to configure your own profile tree
                    // Advanced = new AdvancedTreeConfig
                    // {
                    //     MaxDepth = 20,
                    //     MaxBufferSize = 64,
                    //     CanopyDepth = 14
                    // }
                }
            };

            // Create the create profiles tree transaction
            var txResponse = await StateManager.Instance.Client.CreateCreateProfilesTreeTransaction(createProfilesTreeParams); // txResponse now contains the transaction response, which you'll need to sign and send
            var txHelper = new TransactionHelper(txResponse.CreateCreateProfilesTreeTransaction.Tx);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                resultText.color = Color.green;
                Debug.Log("Profiles tree created successfully");
                Debug.Log(res.ToString());
                resultText.text = "Profiles tree created successfully";

            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to create profiles tree";
                Debug.LogError("Failed to create profiles tree");
            }
        }


        public async void CreateProfile()
        {
            Console.WriteLine("Creating Profile");
            Console.WriteLine("Profile Address: " + profileAddress);

            var authmsg = await StateManager.Instance.Client.AuthRequest(new AuthRequestParams
            {
                Wallet = Web3.Wallet.Account.PublicKey.ToString(),
            });
            var msgToSign = authmsg.AuthRequest.Message.Trim().Replace("\u200B", "");
            var messageBytes = Encoding.UTF8.GetBytes(msgToSign);
            //Debug.Log(msgToSign);
            var sign = await Web3.Wallet.SignMessage(messageBytes);
            // Debug.Log("Signature Valid: " + Web3.Wallet.Account.PublicKey.Verify(messageBytes, sign));

            //Send the signed message to the server
            var authToken = await StateManager.Instance.Client.AuthConfirm(new AuthConfirmParams
            {
                Wallet = Web3.Wallet.Account.PublicKey.ToString(),
                Signature = Encoders.Base58.EncodeData(sign)
            });

            // accessToken
            var accessToken = authToken.AuthConfirm.AccessToken;
            var newProfileTransaction = await StateManager.Instance.Client.CreateNewProfileTransaction(new CreateNewProfileTransactionParams
            {
                Project = projectAddress, // The project's public key
                Payer = authorityPublicKey, // The transaction payer's public key, the profile will also be created for this payer
                Identity = "main", // Identity type in string, the value depends on the project's needs
                Info = new ProfileInfoInput // Optional, profile information, all values in the object are optional
                {
                    Bio = "My name is John Doe",
                    Name = "John Doe",
                    Pfp = "link-to-pfp"
                }

            },
            authToken: accessToken // Required, you'll need to authenticate the user with our Edge Client and provide the resulting access token here, otherwise this operation will fail

        );

            var txResponse = newProfileTransaction.CreateNewProfileTransaction; // This is the transaction response, you'll need to sign and send this transaction

            var txHelper = new TransactionHelper(txResponse);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                resultText.color = Color.green;
                Debug.Log("Profile created successfully");
                Debug.Log(res.ToString());
                resultText.text = "Profile created successfully";
                getandSaveProfileandUserID();

            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to create profile";
                Debug.LogError("Failed to create profile");
            }

        }



        public async void UpdateProfile()
        {

            try
            {
                if (UserID == 0 || profileAddress == "")
                {
                    getandSaveProfileandUserID();
                }

                var authmsg = await StateManager.Instance.Client.AuthRequest(new AuthRequestParams
                {
                    Wallet = Web3.Wallet.Account.PublicKey.ToString(),
                });
                var msgToSign = authmsg.AuthRequest.Message.Trim().Replace("\u200B", "");
                var messageBytes = Encoding.UTF8.GetBytes(msgToSign);
                //Debug.Log(msgToSign);
                var sign = await Web3.Wallet.SignMessage(messageBytes);
                // Debug.Log("Signature Valid: " + Web3.Wallet.Account.PublicKey.Verify(messageBytes, sign));

                //Send the signed message to the server
                var authToken = await StateManager.Instance.Client.AuthConfirm(new AuthConfirmParams
                {
                    Wallet = Web3.Wallet.Account.PublicKey.ToString(),
                    Signature = Encoders.Base58.EncodeData(sign)
                });

                // accessToken
                var accessToken = authToken.AuthConfirm.AccessToken;
                var createUpdateProfileTransaction = await StateManager.Instance.Client.CreateUpdateProfileTransaction(
                    new CreateUpdateProfileTransactionParams
                    {
                        Payer = authorityPublicKey.ToString(), // Payer public key as a string
                        Profile = profileAddress.ToString(), // Profile address as a string
                        Info = new ProfileInfoInput
                        {
                            Bio = "This is profile of user",
                            Name = "User",
                            Pfp = "link-to-pfp"
                        },
                        CustomData = new CustomDataInput
                        {
                            Add = new Dictionary<string, string[]>
                            {
                            { "location", new[] { "San Francisco, CA" } },
                            { "website", new[] { "https://johndoe.dev" } },
                            { "github", new[] { "https://github.com/johndoe" } },
                            { "stars", new[] { "55" } }
                            },
                            Remove = new List<string>
                            {
                            "collaborations" // Removes the key "collaborations" from the profile
                            }
                        }
                    },
                    accessToken
                );

                var txResponse = createUpdateProfileTransaction.CreateUpdateProfileTransaction; // This is the transaction response, you'll need to sign and send this transaction

                var txHelper = new TransactionHelper(txResponse);
                var signedTx = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    resultText.color = Color.green;
                    Debug.Log("Profile updated successfully");

                    resultText.text = "Profile updated successfully";
                }
                else
                {

                    resultText.color = Color.red;
                    resultText.text = "Failed to update profile";
                    Debug.LogError("Failed to update profile");
                    Debug.Log(res.ToString());

                }

            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }


        public async void CreateCharacterModel()
        {
            var merkleTreeAddress = Web3.Wallet.Account.PublicKey.ToString();
            var config = new CharacterConfigInput
            {
                Kind = "Wrapped",
                Criterias = new List<AssetCriteriaInput>
            {
                new AssetCriteriaInput
                {
                    Kind = "Creator", // Can be Collection, Creator, or MerkleTree
                    Params = merkleTreeAddress.ToString() // Provide the relevant address here
                }
            }
            };

            var createCharacterModelTransaction = new CreateCreateCharacterModelTransactionParams
            {
                Authority = adminPublicKey.ToString(), // Project authority public key as a string
                Project = projectAddress.ToString(),
                Payer = adminPublicKey.ToString(), // Optional, if you want to pay from a different wallet
                Config = config
            };

            // Execute the transaction
            var txResponse = await StateManager.Instance.Client.CreateCreateCharacterModelTransaction(createCharacterModelTransaction);
            var txHelper = new TransactionHelper(txResponse.CreateCreateCharacterModelTransaction.Tx);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                characterModelAddress = txResponse.CreateCreateCharacterModelTransaction.CharacterModel; // Address of the character model
                Debug.Log(txResponse.CreateCreateCharacterModelTransaction.CharacterModel + " Character Model Address Saved");
                Debug.Log("Character model created successfully");
                Debug.Log(res.ToString());
                resultText.text = "Character model created successfully";

                var characterModelsReturn = await StateManager.Instance.Client.FindCharacterModels(new FindCharacterModelsParams
                {
                    Addresses = new List<string> { characterModelAddress.ToString() }
                });

                var characterModel = characterModelsReturn.CharacterModel[0];
                characterModelAddress = characterModel.Address.ToString();
                var treeConfig = new TreeSetupConfig
                {
                    // Provide either the basic or advanced configuration
                    Basic = new BasicTreeConfig
                    {
                        NumAssets = 128 // The desired number of characters this tree will be able to store
                    },
                    // Uncomment the following config if you want to configure your own profile tree
                    // Advanced = new AdvancedTreeConfig
                    // {
                    //     CanopyDepth = 20,
                    //     MaxBufferSize = 64,
                    //     MaxDepth = 14
                    // }
                };

                var createCharactersTreeTransaction = new CreateCreateCharactersTreeTransactionParams
                {
                    Authority = adminPublicKey.ToString(),
                    Project = projectAddress.ToString(),
                    CharacterModel = characterModelAddress.ToString(),
                    Payer = adminPublicKey.ToString(), // Optional, only use if you want to pay from a different wallet
                    TreeConfig = treeConfig
                };

                // Execute the transaction
                var txResponseforTree = await StateManager.Instance.Client.CreateCreateCharactersTreeTransaction(createCharactersTreeTransaction);
                var txHelperforTree = new TransactionHelper(txResponseforTree.CreateCreateCharactersTreeTransaction.Tx);
                var signedTxforTree = await txHelperforTree.Sign();
                var resforTree = await StateManager.Instance.Client.SendBulkTransactions(signedTxforTree.ToSendParams());
                if (resforTree.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("Characters tree created successfully");
                    Debug.Log(resforTree.ToString());
                    resultText.text = "Characters tree created successfully";
                    trees = characterModel.Merkle_trees.Merkle_trees.Select(x => x.ToString()).ToList();
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to create characters tree";
                    Debug.LogError("Failed to create characters tree");
                }

            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to create character model";
                Debug.LogError("Failed to create character model");
            }

        }


        public async void CreateCharacterModelAssembled()
        {
            var treeConfig = new TreeSetupConfig
            {
                // This tree is used to store character traits and their necessary information
                Basic = new BasicTreeConfig
                {
                    // The desired number of character information this tree will be able to store
                    NumAssets = 100000
                },
                // Uncomment the following config if you want to configure your own profile tree
                // Advanced = new AdvancedTreeConfig
                // {
                //     MaxDepth = 20, // Max depth of the tree
                //     MaxBufferSize = 64, // Max buffer size of the tree
                //     CanopyDepth = 14, // Canopy depth of the tree
                // }
            };

            var configTransaction = new CreateCreateAssemblerConfigTransactionParams
            {
                Project = projectAddress,
                Authority = adminPublicKey,
                Payer = adminPublicKey, // Optional payer
                TreeConfig = treeConfig,
                Ticker = "unique-string-id", // Provide a unique ticker ID as a string (the ticker ID only needs to be unique within the project)
                Order = new List<string> { "Weapon", "Armor", "Clothes", "Shield" }  // Provide the character traits here; the order matters, in the example order, the background image will be applied and then the skin, expression, clothes, armor, weapon, and shield (if you need your character's expression to appear over the skin, the skin needs to come first in the order)
            };

            var txResponse = await StateManager.Instance.Client.CreateCreateAssemblerConfigTransaction(configTransaction);
            var txHelper = new TransactionHelper(txResponse.CreateCreateAssemblerConfigTransaction.Tx);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                var assemblerConfigAddress = txResponse.CreateCreateAssemblerConfigTransaction.AssemblerConfig;
                Debug.Log("Character model assembled successfully");
                Debug.Log(res.ToString());
                resultText.text = "Character model assembled successfully";
                var traits = new List<CharacterTraitInput>
            {
                // Example traits given below, the labels have to match what you've declared in the assembler config
                new CharacterTraitInput
                {
                    Layer = "Weapon",
                    Name = "Bow",
                    Uri = "https://arweave.net/cpU6oOV7Bf1MM8VDKWp9tYMl0Zq7QB3Axsa5goZYtGg"
                },
                new CharacterTraitInput
                {
                    Layer = "Weapon",
                    Name = "Sword",
                    Uri = "https://arweave.net/cpU6oOV7Bf1MM8VDKWp9tYMl0Zq7QB3Axsa5goZYtGg"
                },
                new CharacterTraitInput
                {
                    Layer = "Armor",
                    Name = "Helmet",
                    Uri = "https://arweave.net/cpU6oOV7Bf1MM8VDKWp9tYMl0Zq7QB3Axsa5goZYtGg"
                },
                new CharacterTraitInput
                {
                    Layer = "Armor",
                    Name = "Chestplate",
                    Uri = "https://arweave.net/cpU6oOV7Bf1MM8VDKWp9tYMl0Zq7QB3Axsa5goZYtGg"
                }
            };

                var addCharacterTraitsTransaction = new CreateAddCharacterTraitsTransactionsParams
                {
                    Traits = traits,
                    AssemblerConfig = assemblerConfigAddress.ToString(),
                    Authority = adminPublicKey.ToString(),
                    Payer = adminPublicKey.ToString()
                };

                // Execute the transaction
                var traitsResponse = await StateManager.Instance.Client.CreateAddCharacterTraitsTransactions(addCharacterTraitsTransaction);
                var traitsHelper = new TransactionHelper(traitsResponse.CreateAddCharacterTraitsTransactions);
                var signedTraits = await traitsHelper.Sign();
                var traitsRes = await StateManager.Instance.Client.SendBulkTransactions(signedTraits.ToSendParams());
                if (traitsRes.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("Character traits added successfully");
                    Debug.Log(traitsRes.ToString());
                    resultText.text = "Character traits added successfully";
                    var config = new CharacterConfigInput
                    {
                        Kind = "Assembled",
                        AssemblerConfigInput = new AssemblerConfigInput
                        {
                            AssemblerConfig = assemblerConfigAddress.ToString(),
                            CollectionName = "Assembled NFT Collection",
                            Name = "Assembled Character NFT 0",
                            Symbol = "ACNFT",
                            Description = "Creating this NFT with assembler",
                            Creators = new List<NftCreatorInput>
                        {
                            new NftCreatorInput
                            {
                                Address = adminPublicKey.ToString(),
                                Share = 100
                            }
                        },
                            SellerFeeBasisPoints = 0
                        }
                    };

                    var createCharacterModelTransactionParams = new CreateCreateCharacterModelTransactionParams
                    {
                        Project = projectAddress.ToString(),
                        Authority = adminPublicKey.ToString(),
                        Payer = adminPublicKey.ToString(), // Optional, use this if you want a different wallet to pay the transaction fee
                        Config = config,

                    };
                    // Execute the transaction

                    var txResponseForCharacterModel = await StateManager.Instance.Client.CreateCreateCharacterModelTransaction(createCharacterModelTransactionParams);
                    var character_model_address = txResponseForCharacterModel.CreateCreateCharacterModelTransaction.CharacterModel; // Address of the character model

                    var txHelperForCharacterModel = new TransactionHelper(txResponseForCharacterModel.CreateCreateCharacterModelTransaction.Tx);
                    var signedTxForCharacterModel = await txHelperForCharacterModel.Sign();
                    var resForCharacterModel = await StateManager.Instance.Client.SendBulkTransactions(signedTxForCharacterModel.ToSendParams());
                    if (resForCharacterModel.SendBulkTransactions[0].Error == null)
                    {

                        Debug.Log("Character model created successfully");
                        Debug.Log(resForCharacterModel.ToString());
                        resultText.text = "Character model created successfully";
                        var treeConfigforCharacterModel = new TreeSetupConfig
                        {
                            // Tree configuration, this affects how many characters this tree can store
                            Basic = new BasicTreeConfig
                            {
                                NumAssets = 100000
                            },
                            // Uncomment the following config if you want to configure your own profile tree
                            // Advanced = new AdvancedTreeConfig
                            // {
                            //     MaxDepth = 3,
                            //     MaxBufferSize = 8,
                            //     CanopyDepth = 3,
                            // }
                        };

                        var createCharactersTreeTransaction = new CreateCreateCharactersTreeTransactionParams
                        {
                            Authority = adminPublicKey.ToString(),
                            Project = projectAddress.ToString(),
                            CharacterModel = character_model_address.ToString(),
                            Payer = adminPublicKey.ToString(), // Optional, only use if you want to pay from a different wallet
                            TreeConfig = treeConfig
                        };

                        // Execute the transaction
                        var txResponseforCharacterTree = await StateManager.Instance.Client.CreateCreateCharactersTreeTransaction(createCharactersTreeTransaction);
                        var txHelperforCharacterTree = new TransactionHelper(txResponseforCharacterTree.CreateCreateCharactersTreeTransaction.Tx);
                        var signedTxforCharacterTree = await txHelperforCharacterTree.Sign();
                        var resforCharacterTree = await StateManager.Instance.Client.SendBulkTransactions(signedTxforCharacterTree.ToSendParams());
                        if (resforCharacterTree.SendBulkTransactions[0].Error == null)
                        {
                            var characterReturn = await StateManager.Instance.Client.FindCharacterModels(new FindCharacterModelsParams
                            {
                                Addresses = new List<string> { character_model_address.ToString() }
                            });

                            var characterModelassemble = characterReturn.CharacterModel[0];


                            Debug.Log("Characters tree created successfully");
                            Debug.Log(resforCharacterTree.ToString());
                            resultText.text = "Characters tree created successfully";
                            var CharacterTreeAssemble = characterModelassemble.Merkle_trees.Merkle_trees.Select(x => x.ToString()).ToList();
                            var attributes = new List<List<string>>
                    {
                        // Attributes for the character
                        new List<string> { "Weapon", "Bow" },
                        new List<string> { "Armor", "Helmet" }
                    };

                            characterModelAddress = character_model_address.ToString();

                            var assembleCharacterTransaction = new CreateAssembleCharacterTransactionParams
                            {
                                Project = projectAddress.ToString(),

                                AssemblerConfig = assemblerConfigAddress.ToString(),
                                CharacterModel = character_model_address.ToString(),
                                Owner = authorityPublicKey.ToString(),
                                Authority = adminPublicKey.ToString(),


                                Attributes = attributes
                            };

                            // Execute the transaction
                            var assembleCharacterResponse = await StateManager.Instance.Client.CreateAssembleCharacterTransaction(assembleCharacterTransaction);
                            var txHelperForAssembleCharacter = new TransactionHelper(assembleCharacterResponse.CreateAssembleCharacterTransaction);
                            var signedTxForAssembleCharacter = await txHelperForAssembleCharacter.Sign();
                            var resForAssembleCharacter = await StateManager.Instance.Client.SendBulkTransactions(signedTxForAssembleCharacter.ToSendParams());
                            if (resForAssembleCharacter.SendBulkTransactions[0].Error == null)
                            {
                                Debug.Log("Character assembled successfully");
                                Debug.Log(resForAssembleCharacter.ToString());
                                resultText.text = "Character assembled successfully";
                            }
                            else
                            {
                                resultText.color = Color.red;
                                resultText.text = "Failed to assemble character";
                                Debug.LogError("Failed to assemble character");
                            }
                        }
                        else
                        {
                            resultText.color = Color.red;
                            resultText.text = "Failed to create characters tree";
                            Debug.LogError("Failed to create characters tree");
                        }

                    }
                    else
                    {
                        resultText.color = Color.red;
                        resultText.text = "Failed to create character model";
                        Debug.LogError("Failed to create character model");
                    }
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to add character traits";
                    Debug.LogError("Failed to add character traits");
                }









            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to assemble character model";
                Debug.LogError("Failed to assemble character model");
            }

        }
        public async void WrapAssets()
        {

            if (nftMinAddress == "")
            {
                CreateNFTMint();
            }
            var wrapAssetsToCharacterTransaction = new CreateWrapAssetsToCharacterTransactionsParams
            {
                Project = projectAddress.ToString(),
                MintList = new List<string>{
                // Provide NFT addresses here
                nftMinAddress.ToString()
            },
                Wallet = authorityPublicKey.ToString(), // User's wallet public key as a string
                CharacterModel = characterModelAddress.ToString()
            };

            // Execute the transaction
            var txResponse = await StateManager.Instance.Client.CreateWrapAssetsToCharacterTransactions(wrapAssetsToCharacterTransaction);
            var txHelper = new TransactionHelper(txResponse.CreateWrapAssetsToCharacterTransactions);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                Debug.Log("Assets wrapped successfully");
                Debug.Log(res.ToString());
                resultText.text = "Assets wrapped successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to wrap assets";
                Debug.LogError("Failed to wrap assets");
            }
        }


        public async void UnwrapAssets()
        {

            var charactersReturn = await StateManager.Instance.Client.FindCharacters(new FindCharactersParams
            {
                Wallets = new List<string> { authorityPublicKey.ToString() },
                Trees = trees,
            });

            if (charactersReturn.Character.Count == 0)
            {
                resultText.color = Color.red;
                resultText.text = "No characters found";
                Debug.LogError("No characters found");
                return;
            }

            Debug.Log(charactersReturn.Character[0].Address.ToString() + " Character Address Saved");


            var unwrapAssetsFromCharacterTransaction = new CreateUnwrapAssetsFromCharacterTransactionsParams
            {
                CharacterAddresses = new List<string>
            {
                // Provide character addresses here
                charactersReturn.Character[0].Address.ToString()
            },
                CharacterModel = characterModelAddress.ToString(),
                Project = projectAddress.ToString(),
                Wallet = authorityPublicKey.ToString(),
                //LibreplexDeployment = libreplexDeploymentAddress.ToString() // Optional, Libreplex deployment public key as a string
            };

            // Execute the transaction
            await StateManager.Instance.Client.CreateUnwrapAssetsFromCharacterTransactions(unwrapAssetsFromCharacterTransaction);
            var txResponse = await StateManager.Instance.Client.CreateUnwrapAssetsFromCharacterTransactions(unwrapAssetsFromCharacterTransaction);
            var txHelper = new TransactionHelper(txResponse.CreateUnwrapAssetsFromCharacterTransactions);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                Debug.Log("Assets unwrapped successfully");
                Debug.Log(res.ToString());
                resultText.text = "Assets unwrapped successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to unwrap assets";
                Debug.LogError("Failed to unwrap assets");
            }
        }


        public async void CreateUnCompressedResource()
        {


            var createNewResourceTransaction = await StateManager.Instance.Client.CreateCreateNewResourceTransaction(new CreateCreateNewResourceTransactionParams
            {
                Project = projectAddress,
                Authority = adminPublicKey,
                //DelegateAuthority = delegateAuthority, // Optional, resource delegate authority public key
                Payer = adminPublicKey, // Optional, specify when you want a different wallet to pay for the tx
                Params = new InitResourceInput
                {
                    Name = "Gold", // Name of the resource
                    Decimals = 6, // Number of decimal places the resource can be divided into
                    Symbol = "GOLD", // Symbol of the resource
                    Uri = "https://example.com", // URI of the resource
                    Storage = ResourceStorageEnum.AccountState // Type of the resource, can be either AccountState (uncompressed/unwrapped) or LedgerState (compressed/wrapped)
                }
            });


            var txResponse = createNewResourceTransaction.CreateCreateNewResourceTransaction.Tx; // This is the transaction response, you'll need to sign and send this transaction
            var txHelper = new TransactionHelper(txResponse);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                resourceAddress = createNewResourceTransaction.CreateCreateNewResourceTransaction.Resource; // Address of the resource
                Debug.Log("Resource created successfully");
                Debug.Log(res.ToString());
                resultText.text = "Resource created successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to create resource";
                Debug.LogError("Failed to create resource");
            }

        }

        public async void CreateCompressedResource()
        {


            var createNewResourceTransaction = await StateManager.Instance.Client.CreateCreateNewResourceTransaction(new CreateCreateNewResourceTransactionParams
            {
                Project = projectAddress,
                Authority = adminPublicKey,
                //DelegateAuthority = "delegateAuthorityPublicKey", // Optional, resource delegate authority public key
                Payer = adminPublicKey, // Optional, specify when you want a different wallet to pay for the tx
                Params = new InitResourceInput
                {
                    Name = "Gold", // Name of the resource
                    Decimals = 6, // Number of decimal places the resource can be divided into
                    Symbol = "GOLD", // Symbol of the resource
                    Uri = "https://example.com", // URI of the resource
                    Storage = ResourceStorageEnum.LedgerState // Type of the resource, can be either AccountState (uncompressed/unwrapped) or LedgerState (compressed/wrapped)
                }
            });


            var txResponse = createNewResourceTransaction.CreateCreateNewResourceTransaction.Tx; // This is the transaction response, you'll need to sign and send this transaction
            var txHelper = new TransactionHelper(txResponse);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                resourceAddress = createNewResourceTransaction.CreateCreateNewResourceTransaction.Resource; // This is the resource address once it'll be created
                Debug.Log("Resource created successfully");
                Debug.Log(res.ToString());
                resultText.text = "Resource created successfully";
                var createNewResourceTreeTransaction = await StateManager.Instance.Client.CreateCreateNewResourceTreeTransaction(new CreateCreateNewResourceTreeTransactionParams
                {
                    Project = projectAddress,
                    Authority = adminPublicKey,
                    //DelegateAuthority = delegateAuthorityPublicKey, // Optional
                    Payer = adminPublicKey, // Optional, specify when you want a different wallet to pay for the tx
                    Resource = resourceAddress,
                    TreeConfig = new TreeSetupConfig
                    {
                        // Provide either the basic or advanced configuration, we recommend using the basic configuration if you don't know the exact values of maxDepth, maxBufferSize, and canopyDepth (the basic configuration will automatically configure these values for you)
                        Basic = new BasicTreeConfig
                        {
                            NumAssets = 128 // The desired number of resources this tree will be able to store
                        }
                        // Uncomment the following config if you want to configure your own profile tree (also comment out the above config)
                        // Advanced = new AdvancedTreeConfig
                        // {
                        //     MaxDepth = 20,
                        //     MaxBufferSize = 64,
                        //     CanopyDepth = 14
                        // }
                    }
                });

                var merkleTreeAddress = createNewResourceTreeTransaction.CreateCreateNewResourceTreeTransaction.TreeAddress; // This is the merkle tree address once it'll be created
                var txResponseforMerkleTree = createNewResourceTreeTransaction.CreateCreateNewResourceTreeTransaction.Tx; // This is the transaction response, you'll need to sign and send this transaction
                var txHelperforMerkleTree = new TransactionHelper(txResponseforMerkleTree);
                var signedTxforMerkleTree = await txHelperforMerkleTree.Sign();
                var resforMerkleTree = await StateManager.Instance.Client.SendBulkTransactions(signedTxforMerkleTree.ToSendParams());
                if (resforMerkleTree.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("Resource tree created successfully");
                    Debug.Log(resforMerkleTree.ToString());
                    resultText.text = "Resource tree created successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to create resource tree";
                    Debug.LogError("Failed to create resource tree");
                }
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to create resource";
                Debug.LogError("Failed to create resource");
            }

        }





        public async void MintResource()
        {
            if (resourceAddress == "")
            {
                CreateCompressedResource();
            }

            var createMintResourceTransaction = await StateManager.Instance.Client.CreateMintResourceTransaction(new CreateMintResourceTransactionParams
            {
                Resource = resourceAddress, // Resource public key as a string
                Amount = "50000", // Amount of the resource to mint
                Authority = adminPublicKey, // Project authority's public key
                Owner = authorityPublicKey, // The owner's public key, this wallet will receive the resource
                                            //DelegateAuthority = delegateAuthority, // Optional, if specified, the delegate authority will be used to mint the resource
                Payer = adminPublicKey // Optional, specify when you want a different wallet to pay for the tx
            });

            var txResponse = createMintResourceTransaction.CreateMintResourceTransaction; // This is the transaction response, you'll need to sign and send this transaction
            var txHelper = new TransactionHelper(txResponse);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                Debug.Log("Resource minted successfully");
                Debug.Log(res.ToString());
                resultText.text = "Resource minted successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to mint resource";
                Debug.LogError("Failed to mint resource");
            }
        }

        public async void BurnResource()
        {

            if (resourceAddress == "")
            {
                CreateCompressedResource();
            }

            var createBurnResourceTransaction = await StateManager.Instance.Client.CreateBurnResourceTransaction(new CreateBurnResourceTransactionParams
            {
                Authority = authorityPublicKey, // The resource owner's public key
                Resource = resourceAddress,
                Amount = "50000", // Amount of the resource to burn
                Payer = adminPublicKey // Optional, specify when you want a different wallet to pay for the tx
            }); ;

            var txResponse = createBurnResourceTransaction.CreateBurnResourceTransaction; // This is the transaction response, you'll need to sign and send this transaction
            var txHelper = new TransactionHelper(txResponse);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                Debug.Log("Resource burned successfully");
                Debug.Log(res.ToString());
                resultText.text = "Resource burned successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to burn resource";
                Debug.LogError("Failed to burn resource");
            }
        }


        public async void CreateStakingPool()
        {
            var createStakingPoolTransaction = await StateManager.Instance.Client.CreateCreateStakingPoolTransaction(new CreateCreateStakingPoolTransactionParams
            {

                Project = projectAddress.ToString(),
                Resource = resourceAddress.ToString(), // Resource's pubkey address in string format
                Authority = adminPublicKey.ToString(),

                // DelegateAuthority = delegateAuthority.ToString(),
                Payer = payerPublicKey.ToString(),
                Multiplier = new InitStakingMultiplierMetadataInput
                {
                    Decimals = 2,
                    Multipliers = new List<AddMultiplierMetadataInput>
                {
                    new AddMultiplierMetadataInput
                    {
                        Value = "10",
                        Type = new MultiplierTypeInput
                        {
                            Collection =  collection.ToString(),
                        }
                    },
                    new AddMultiplierMetadataInput
                    {
                        Value = "5",
                        Type = new MultiplierTypeInput
                        {
                            Creator = creatorPublicKey.ToString(),
                        }
                    },
                    new AddMultiplierMetadataInput
                    {
                        Value = "2",
                        Type = new MultiplierTypeInput
                        {
                            MinNftCount = "1",
                        }
                    },
                    new AddMultiplierMetadataInput
                    {
                        Value = "3",
                        Type = new MultiplierTypeInput
                        {
                            MinStakeDuration = "1",
                        }
                    }
                }
                },
                Metadata = new CreateStakingPoolMetadataInput
                {
                    Name = "Staking", // Staking pool name
                    RewardsPerDuration = "1", // Rewards per duration
                    RewardsDuration = "1", // Rewards duration in seconds
                    MaxRewardsDuration = null, // Maximum rewards duration in seconds
                    MinStakeDuration = null, // Minimum stake duration in seconds
                    CooldownDuration = null, // Rewards cooldown
                    ResetStakeDuration = false, // Reset stake duration
                    StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), // Staking pool start time in UNIX seconds
                    EndTime = null, // Staking pool endtime in UNIX seconds, null if not set
                    LockType = LockTypeEnum.Freeze // Lock type for the staking pool
                }
            });

            var transactions = createStakingPoolTransaction.CreateCreateStakingPoolTransaction.Transactions; // Transaction response, to be signed and sent

            var txHelper = new TransactionHelper(transactions);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                stakingPoolAddress = createStakingPoolTransaction.CreateCreateStakingPoolTransaction.StakingPoolAddress; // Address of the staking pool
                multipliersAddress = createStakingPoolTransaction.CreateCreateStakingPoolTransaction.MultipliersAddress; // Address of the multipliers
                resultText.color = Color.green;
                Debug.Log("Staking pool created successfully");
                Debug.Log(res.ToString());
                resultText.text = "Staking pool created successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to create staking pool";
                Debug.LogError("Failed to create staking pool");
            }

        }


        public async void UpdateStakingPool()
        {


            var updateStakingPoolParams = new CreateUpdateStakingPoolTransactionParams
            {
                Project = projectAddress.ToString(), // Project address
                Authority = adminPublicKey.ToString(), // Authority public key
                Resource = resourceAddress.ToString(), // Optional: resource address
                //DelegateAuthority = delegateAuthority.ToString(), // Optional: delegate authority public key
                StakingPool = stakingPoolAddress.ToString(), // Staking pool address
                CharacterModel = characterModelAddress.ToString(), // Optional: character model address
                Payer = payerPublicKey.ToString(), // Optional: transaction payer public key
                // Metadata = new UpdateStakingPoolMetadataInput // Optional: metadata object
                // {
                //     CooldownDuration = "0", // Duration in seconds, UNIX format
                //     EndTime = null, // Staking pool end time, send null if not setting an end time
                //     LockType = LockTypeEnum.Freeze, // Lock type for the staking pool
                //     MaxRewardsDuration = "9600", // Maximum rewards duration in seconds
                //     MinStakeDuration = "3600", // Minimum stake duration in seconds
                //     Name = "New name", // Staking pool name
                //     ResetStakeDuration = "86400", // Reset stake duration in seconds
                //     RewardsDuration = "3600", // Rewards duration in seconds
                //     RewardsPerDuration = "100", // Rewards per duration
                //     StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), // Staking pool start time
                // }
            };

            // Create update staking pool transaction
            var txResponse = await StateManager.Instance.Client.CreateUpdateStakingPoolTransaction(updateStakingPoolParams);

            // Extract transaction response
            var transactionResponse = txResponse.CreateUpdateStakingPoolTransaction; // The transaction response
            var txHelper = new TransactionHelper(transactionResponse); // The transaction helper
            var signedTx = await txHelper.Sign(); // Sign the transaction
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams()); // Send the transaction
            if (res.SendBulkTransactions[0].Error == null)
            {
                resultText.color = Color.green;
                Debug.Log("Staking pool updated successfully");
                Debug.Log(res.ToString());
                resultText.text = "Staking pool updated successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to update staking pool";
                Debug.LogError("Failed to update staking pool");


            }
        }

        public async void CreateMultipliers()
        {

            var initMultipliersTransaction = await StateManager.Instance.Client.CreateInitMultipliersTransaction(new CreateInitMultipliersTransactionParams
            {
                Project = projectAddress.ToString(),
                Authority = adminPublicKey.ToString(),
                StakingPool = stakingPoolAddress.ToString(),
                //empty list in Multipliers
                Multipliers = new List<AddMultiplierMetadataInput>(),
                Decimals = 3
            });

            var tx = initMultipliersTransaction.CreateInitMultipliersTransaction.Tx; // Transaction response, to be signed and sent
            multipliersAddress = initMultipliersTransaction.CreateInitMultipliersTransaction.MultipliersAddress; // Address of the multipliers
            var txHelper = new TransactionHelper(tx);
            var signedTx = await txHelper.Sign();
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
            if (res.SendBulkTransactions[0].Error == null)
            {
                resultText.color = Color.green;
                Debug.Log("Multipliers created successfully");
                Debug.Log(res.ToString());
                resultText.text = "Multipliers created successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to create multipliers";
                Debug.LogError("Failed to create multipliers");
            }

        }

        public async void AddMultipliers()
        {
            var addMultiplierParams = new CreateAddMultiplierTransactionParams
            {
                Project = projectAddress, // Project address
                Authority = adminPublicKey.ToString(), // Authority public key
                Multiplier = multipliersAddress.ToString(), // Existing multipliers address
                Payer = payerPublicKey.ToString(), // Optional: transaction payer public key
                                                   //DelegateAuthority = delegateAuthority.ToString(), // Optional: delegate authority public key
                Metadata =
                    new AddMultiplierMetadataInput
                    {
                        Value = "300",
                        Type = new MultiplierTypeInput
                        {
                            Creator = authorityPublicKey.ToString(),
                        }
                    },
            };

            // Create add multiplier transaction
            var txResponse = await StateManager.Instance.Client.CreateAddMultiplierTransaction(addMultiplierParams); // The transaction response
            var txHelper = new TransactionHelper(txResponse.CreateAddMultiplierTransaction); // The transaction helper
            var signedTx = await txHelper.Sign(); // Sign the transaction
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams()); // Send the transaction
            if (res.SendBulkTransactions[0].Error == null)
            {
                resultText.color = Color.green;
                Debug.Log("Multipliers added successfully");
                Debug.Log(res.ToString());
                resultText.text = "Multipliers added successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to add multipliers";
                Debug.LogError("Failed to add multipliers");
            }
        }


        public async void StakeCharacters()
        {
            var charactersReturn = await StateManager.Instance.Client.FindCharacters(new FindCharactersParams
            {
                Filters = new CharactersFilter
                {
                    Owner = authorityPublicKey.ToString(),
                    UsedBy = {
                        Kind = "None"
                    }
                },
                Trees = trees
            });

            if (charactersReturn.Character.Count == 0)
            {
                resultText.color = Color.red;
                resultText.text = "No characters found";
                Debug.LogError("No characters found");
                return;
            }

            Debug.Log(charactersReturn.Character[0].Address.ToString() + " Character Address Saved");
            characterAddress = charactersReturn.Character[0].Address.ToString();
            var stakeCharactersParams = new CreateStakeCharactersTransactionsParams
            {
                Project = projectAddress.ToString(), // Project address
                StakingPool = stakingPoolAddress.ToString(), // Staking pool address
                CharacterModel = characterModelAddress.ToString(), // Character model address
                CharacterAddresses = new List<string>
                {
                   charactersReturn.Character[0].Address.ToString() // Character address
                },
                FeePayer = payerPublicKey.ToString() // Fee payer public key
            };

            // Create stake characters transactions
            var txResponse = await StateManager.Instance.Client.CreateStakeCharactersTransactions(stakeCharactersParams); // This will return the transaction response
            var txHelper = new TransactionHelper(txResponse.CreateStakeCharactersTransactions); // The transaction helper
            var signedTx = await txHelper.Sign(); // Sign the transaction
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams()); // Send the transaction
            if (res.SendBulkTransactions[0].Error == null)
            {
                resultText.color = Color.green;
                Debug.Log("Characters staked successfully");
                Debug.Log(res.ToString());
                resultText.text = "Characters staked successfully";
            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to stake characters";
                Debug.LogError("Failed to stake characters");
            }

        }

        public async void ClaimStakingRewards()
        {

            if (projectAddress == "")
            {
                resultText.color = Color.red;
                resultText.text = "Please create a project first";
                Debug.LogError("Please create a project first");
                return;
            }
            var claimStakingRewardsParams = new CreateClaimStakingRewardsTransactionsParams
            {
                CharacterAddresses = new List<string>
                {
                    characterAddress.ToString() // Character address
                },
                CharacterModel = characterModelAddress.ToString(), // Character model address
                FeePayer = payerPublicKey.ToString() // Fee payer public key
            };

            // Create claim staking rewards transactions
            var txResponse = await StateManager.Instance.Client.CreateClaimStakingRewardsTransactions(claimStakingRewardsParams); // The transaction response
            var txHelper = new TransactionHelper(txResponse.CreateClaimStakingRewardsTransactions); // The transaction helper
            var signedTx = await txHelper.Sign(); // Sign the transaction
            var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams()); // Send the transaction
            if (res.SendBulkTransactions[0].Error == null)
            {
                resultText.color = Color.green;
                Debug.Log("Staking rewards claimed successfully");
                Debug.Log(res.ToString());
                resultText.text = "Staking rewards claimed successfully";

            }
            else
            {
                resultText.color = Color.red;
                resultText.text = "Failed to claim staking rewards";
                Debug.LogError("Failed to claim staking rewards");

            }
        }



        public async void CreateMissionPool()
        {
            if (characterModelAddress == "")
            {
                resultText.color = Color.red;
                resultText.text = "Please create a character model first";
                Debug.LogError("Please create a character model first");
                return;
            }


            try
            {
                var createMissionPoolParams = new CreateCreateMissionPoolTransactionParams
                {
                    Data = new NewMissionPoolData
                    {
                        Name = "Test Mission Pool 123", // Name of the mission pool
                        Project = projectAddress, // Project address
                        Payer = adminPublicKey, // Public key of the payer
                        Authority = adminPublicKey, // Public key of the project authority
                                                    //DelegateAuthority = delegateAuthority, // Optional
                        CharacterModel = characterModelAddress // Character model address
                    }
                };
                // Create a mission pool transaction
                var tx = await StateManager.Instance.Client.CreateCreateMissionPoolTransaction(createMissionPoolParams);
                // Extract mission pool address and transaction response from the result
                var txResponse = tx.CreateCreateMissionPoolTransaction.Tx; // This is the transaction response, which you'll need to sign and send
                var txHelper = new TransactionHelper(txResponse);
                var signedTx = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    missionPoolAddress = tx.CreateCreateMissionPoolTransaction.MissionPoolAddress; // This is the mission pool address once created
                    resultText.color = Color.green;
                    Debug.Log("Mission pool created successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Mission pool created successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to create mission pool";
                    Debug.LogError("Failed to create mission pool");
                }

            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }


        public async void CreateMission()
        {

            if (missionPoolAddress == "")
            {
                resultText.color = Color.red;
                resultText.text = "Please create a mission pool first";
                Debug.LogError("Please create a mission pool first");
                return;
            }

            if (resourceAddress == "")
            {
                resultText.color = Color.red;
                resultText.text = "Please create a resource first";
                Debug.LogError("Please create a resource first");
                return;

            }

            try
            {
                var createMissionTransaction = await StateManager.Instance.Client.CreateCreateMissionTransaction(new CreateCreateMissionTransactionParams
                {
                    Data = new NewMissionData
                    {
                        Name = "Test mission",
                        Project = projectAddress.ToString(),
                        Cost = new NewMissionCost
                        {
                            Address = resourceAddress.ToString(),
                            Amount = "1"
                        },
                        Duration = "1", // 1 day
                        MinXp = "0", // Minimum XP required to participate in the mission
                        Rewards = new List<MissionReward>
                    {
                        new MissionReward
                        {
                            Kind = RewardKind.Xp,
                            Max = "100",
                            Min = "100"
                        },
                        new MissionReward
                        {
                            Kind = RewardKind.Resource,
                            Max = "500000000",
                            Min = "500000000",
                            Resource = resourceAddress.ToString()
                        }
                    },
                        MissionPool = missionPoolAddress.ToString(),
                        Authority = adminPublicKey.ToString(),
                        Payer = adminPublicKey.ToString()
                    }
                });

                var tx = createMissionTransaction.CreateCreateMissionTransaction.Tx; // Transaction response, to be signed and sent
                var txHelper = new TransactionHelper(tx);
                var signedTx = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());
                if (res.SendBulkTransactions[0].Error == null)
                {
                    missionAddress = createMissionTransaction.CreateCreateMissionTransaction.MissionAddress; // Address of the created mission
                    resultText.color = Color.green;
                    Debug.Log("Mission created successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Mission created successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to create mission";
                    Debug.LogError("Failed to create mission");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);

            }
        }

        public async void SendCharactersOnMission()
        {
            if (profileAddress == "")
            {
                resultText.color = Color.red;
                resultText.text = "Please create a profile first";
                Debug.LogError("Please create a profile first");
                return;
            }
            if (missionAddress == "")
            {
                resultText.color = Color.red;
                resultText.text = "Please create a mission first";
                Debug.LogError("Please create a mission first");
                return;
            }

            try
            {
                var charactersReturn = await StateManager.Instance.Client.FindCharacters(new FindCharactersParams
                {
                    Wallets = new List<string> { authorityPublicKey.ToString() },
                    Trees = trees,
                });

                if (charactersReturn.Character.Count == 0)
                {
                    resultText.color = Color.red;
                    resultText.text = "No characters found";
                    Debug.LogError("No characters found");
                    return;
                }

                charactersReturn.Character[0].Address.ToString();

                var sendCharactersOnMissionParams = new CreateSendCharactersOnMissionTransactionParams
                {
                    Data = new ParticipateOnMissionData
                    {
                        Mission = missionAddress.ToString(), // Mission address
                        CharacterAddresses = new List<string>
                {
                    charactersReturn.Character[0].Address.ToString() // Character address
                },
                        Authority = authorityPublicKey.ToString(), // Public key of the authority
                        Payer = payerPublicKey.ToString(), // Optional: public key of the payer
                    }
                };


                // Create a send characters on mission transaction
                var txResponse = await StateManager.Instance.Client.CreateSendCharactersOnMissionTransaction(sendCharactersOnMissionParams); // This is the transaction response, you'll need to sign and send this transaction 
                                                                                                                                             // Create send characters on mission transactions
                var txHelper = new TransactionHelper(txResponse.CreateSendCharactersOnMissionTransaction);
                var signedTx = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams());


                if (res.SendBulkTransactions[0].Error == null)
                {
                    resultText.color = Color.green;
                    Debug.Log("Characters sent on mission successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Characters sent on mission successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to send characters on mission";
                    Debug.LogError("Failed to send characters on mission");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public async void RecallCharactersFromMission()
        {


            try
            {

                var charactersReturn = await StateManager.Instance.Client.FindCharacters(new FindCharactersParams
                {
                    Wallets = new List<string> { authorityPublicKey.ToString() },
                    Trees = trees,
                });

                if (charactersReturn.Character.Count == 0)
                {
                    resultText.color = Color.red;
                    resultText.text = "No characters found";
                    Debug.LogError("No characters found");
                    return;
                }
                var recallCharactersParams = new CreateRecallCharactersTransactionParams
                {
                    Data = new RecallFromMissionData
                    {
                        Mission = missionAddress.ToString(), // Mission address
                        CharacterAddresses = new List<string>
                {
                    charactersReturn.Character[0].Address.ToString() // Character address
                },
                        Authority = authorityPublicKey.ToString(), // Public key of the authority
                        Payer = payerPublicKey.ToString(), // Optional: public key of the payer
                    },
                    // LutAddresses = new List<string>
                    // {
                    //     lookupTableAddress // Lookup table address
                    // }
                };

                // Create a recall characters transaction
                var txResponse = await StateManager.Instance.Client.CreateRecallCharactersTransaction(recallCharactersParams); // This is the transaction response, you'll need to sign and send this transaction
                                                                                                                               // Extract transaction response
                var txHelper = new TransactionHelper(txResponse.CreateRecallCharactersTransaction); // The transaction helper
                var signedTx = await txHelper.Sign(); // Sign the transaction
                var res = await StateManager.Instance.Client.SendBulkTransactions(signedTx.ToSendParams()); // Send the transaction
                if (res.SendBulkTransactions[0].Error == null)
                {
                    resultText.color = Color.green;
                    Debug.Log("Characters recalled from mission successfully");
                    Debug.Log(res.ToString());
                    resultText.text = "Characters recalled from mission successfully";
                }
                else
                {
                    resultText.color = Color.red;
                    resultText.text = "Failed to recall characters from mission";
                    Debug.LogError("Failed to recall characters from mission");
                }

            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }

        }




        private void OnEnable()
        {
        }

        public override void ShowScreen(object data = null)
        {
            base.ShowScreen();
            gameObject.SetActive(true);
        }

        public override void HideScreen()
        {
            base.HideScreen();
            gameObject.SetActive(false);
        }

        public void OnClose()
        {
            var wallet = GameObject.Find("wallet");
            wallet.SetActive(false);
        }
    }
};
