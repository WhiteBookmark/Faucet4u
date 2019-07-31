<template>
    <v-app>
        <!--template to be used if user is logged in-->
        <div id="app" v-if="sessionValid" class="text-center">
            <header>
                <v-toolbar elevation="24">
                    <v-toolbar-title>
                        <a href="#/Home">
                            <v-img src="Images/Logo.png"
                                   max-width="250"
                                   max-height="50"
                                   alt="Home"></v-img>
                        </a>
                    </v-toolbar-title>
                    <v-spacer></v-spacer>
                    <v-toolbar-items>
                        <v-btn to="/Advertise">Advertise</v-btn>
                        <v-btn to="/Settings">Settings</v-btn>
                        <v-btn to="/Deposit">Deposit</v-btn>
                        <v-btn to="/Withdrawal">Withdraw</v-btn>
                        <v-btn to="/Rates">Rates</v-btn>
                        <v-btn to="/HomeSupport">Support</v-btn>
                        <v-btn :href="faucetSettingValue(ForumLink)">Forum</v-btn>
                        <v-btn @click="logOut()">Log out</v-btn>
                    </v-toolbar-items>
                </v-toolbar>
                <v-progress-linear :active="isLoading" :indeterminate="true" :height="10" color="orange"></v-progress-linear>
            </header>

            <main :style="{ marginTop: '100px' }">
                <!--User navigation-->
                <v-container fluid text-center justify-center align-center pa-0>
                    <v-layout>
                        <v-flex xs3></v-flex>
                        <v-flex>
                            <v-menu offset-y eager open-on-hover bottom origin="center center" transition="slide-y-transition" :close-on-click="true">
                                <template v-slot:activator="{ on }">
                                    <v-btn elevation="24" x-large text ripple block tile v-on="on">Advertising</v-btn>
                                </template>
                                <v-list :dense="true">
                                    <v-list-item-group color="orange">

                                        <v-list-item to="/AdvertiserPanelPTP" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>PTP</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/AdvertiserPanelBanner" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Banner</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/AdvertiserPanelSquareBanner" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Square Banner</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/AdvertiserPanelBonusAds" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Bonus Ads</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                    </v-list-item-group>
                                </v-list>
                            </v-menu>
                        </v-flex>

                        <v-flex>
                            <v-menu offset-y eager open-on-hover bottom origin="center center" transition="slide-y-transition" :close-on-click="true">
                                <template v-slot:activator="{ on }">
                                    <v-btn elevation="24" x-large text ripple block tile v-on="on">Statistics</v-btn>
                                </template>
                                <v-list :dense="true">
                                    <v-list-item-group color="orange">

                                        <v-list-item to="/Account" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Account</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item disabled>
                                            <v-list-item-content>
                                                <v-list-item-title>Charts (Coming soon)</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                    </v-list-item-group>
                                </v-list>
                            </v-menu>
                        </v-flex>

                        <v-flex>
                            <v-menu offset-y eager open-on-hover bottom origin="center center" transition="slide-y-transition" :close-on-click="true">
                                <template v-slot:activator="{ on }">
                                    <v-btn elevation="24" x-large text ripple block tile v-on="on">Earn Money</v-btn>
                                </template>
                                <v-list :dense="true">
                                    <v-list-item-group color="orange">

                                        <v-list-item to="/Home" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Faucet Claim</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/BonusAds" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Bonus Ads</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/PTP" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>PTP</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                    </v-list-item-group>
                                </v-list>
                            </v-menu>
                        </v-flex>

                        <v-flex>
                            <v-menu offset-y eager open-on-hover bottom origin="center center" transition="slide-y-transition" :close-on-click="true">
                                <template v-slot:activator="{ on }">
                                    <v-btn elevation="24" x-large text ripple block tile v-on="on">Tasks</v-btn>
                                </template>
                                <v-list :dense="true">
                                    <v-list-item-group color="orange">

                                        <v-list-item to="/Hitswall" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Hitswall</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/KiwiWall" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>KiwiWall</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/PTCWall" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>PTCWall</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/SkippyAds" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>SkippyAds</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                    </v-list-item-group>
                                </v-list>
                            </v-menu>
                        </v-flex>

                        <v-flex>
                            <v-menu offset-y eager open-on-hover bottom origin="center center" transition="slide-y-transition" :close-on-click="true">
                                <template v-slot:activator="{ on }">
                                    <v-btn elevation="24" x-large text ripple block tile v-on="on">Affiliate</v-btn>
                                </template>
                                <v-list :dense="true">
                                    <v-list-item-group color="orange">

                                        <v-list-item to="/Banners" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Banners</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/Referrals" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Referrals</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                    </v-list-item-group>
                                </v-list>
                            </v-menu>
                        </v-flex>

                        <v-flex>
                            <v-menu offset-y eager open-on-hover bottom origin="center center" transition="slide-y-transition" :close-on-click="true">
                                <template v-slot:activator="{ on }">
                                    <v-btn elevation="24" x-large text ripple block tile v-on="on">History</v-btn>
                                </template>
                                <v-list :dense="true">
                                    <v-list-item-group color="orange">

                                        <v-list-item to="/LoginHistory" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Login</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/DepositHistory" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Deposits</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/WithdrawalHistory" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Withdrawals</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                        <v-list-item to="/OrderHistory" exact>
                                            <v-list-item-content>
                                                <v-list-item-title>Orders</v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>

                                    </v-list-item-group>
                                </v-list>
                            </v-menu>
                        </v-flex>

                    </v-layout>
                </v-container>
                <br />

                <!--Chatbox-->
                <v-container fluid text-center justify-center align-center pa-0>
                    <v-layout>
                        <v-flex xs3>
                            <v-card elevation="24" class="ml-2 mr-2 black--text font-weight-black">
                                <v-card-title height="100px">
                                    <!--Chat rules-->
                                    <v-btn class="mx-auto" elevation="24" x-large outlined ripple tile color="orange" @click="openChatRulesModal()">Chat Rules</v-btn>
                                </v-card-title>
                                <v-divider></v-divider>

                                <v-card-text style="height:450px;" class="pa-0 text-left overflow-y-auto" id="chatMessages">
                                    <v-list class="pa-0">
                                        <v-list-item-group color="orange">
                                            <v-list-item v-for="x in chatHistory">
                                                <v-list-item-content class="pa-0">
                                                    <v-list-item-title>
                                                        <b>{{ x.Username }}</b>
                                                        &nbsp;
                                                        <small>
                                                            {{ x.DateTime | moment("YYYY-MM-DD - hh:mm:ss A") }}
                                                        </small>
                                                    </v-list-item-title>
                                                    <p v-if="x.Message.includes(chatTaggingUsername)"
                                                       class="orange--text">
                                                        {{ x.Message }}
                                                    </p>
                                                    <p v-else>{{ x.Message }}</p>
                                                </v-list-item-content>
                                            </v-list-item>
                                        </v-list-item-group>
                                    </v-list>
                                </v-card-text>
                                <v-divider class="pa-0 ma-0"></v-divider>
                                <v-card-actions style="height: 50px;" class="pa-0">
                                    <v-container fluid text-center justify-center align-baseline pa-0>
                                        <v-layout>

                                            <v-flex xs8>
                                                <div v-if="chatBanned">
                                                    <v-text-field solo flat style="max-height: 48px;" name="chatMessage" v-validate="'required'" v-model="chatMessage" @keyup.enter.native="sendChatBannedMessage()"></v-text-field>
                                                </div>
                                                <div v-else>
                                                    <v-text-field solo flat style="max-height: 48px;" name="chatMessage" v-validate="'required'" v-model="chatMessage" @keyup.enter.native="sendChatMessage()"></v-text-field>
                                                </div>
                                            </v-flex>

                                            <v-flex>
                                                <v-btn ripple block tile :icon="true" @click="sendChatBannedMessage()" v-if="userData.ChatBanned">
                                                    <v-icon x-large color="orange darken-2">mdi-message-text</v-icon>
                                                </v-btn>
                                                <v-btn ripple block tile :icon="true" @click="sendChatMessage()" v-else>
                                                    <v-icon x-large color="orange darken-2">mdi-message-text</v-icon>
                                                </v-btn>
                                            </v-flex>

                                        </v-layout>
                                    </v-container>
                                </v-card-actions>
                            </v-card>
                        </v-flex>
                        <v-flex>
                            <router-view></router-view>
                        </v-flex>
                    </v-layout>
                </v-container>
            </main>
        </div>
        <!--Template to be used if user is not logged in-->
        <div id="app" v-else class="text-center">
            <header>
                <v-toolbar elevation="24">
                    <v-toolbar-title>
                        <a href="#/">
                            <v-img src="Images/Logo.png" class="animated infinite bounce"
                                   max-width="250"
                                   max-height="50"
                                   alt="Home"></v-img>
                        </a>
                    </v-toolbar-title>
                    <v-spacer></v-spacer>
                    <v-toolbar-items>
                        <v-btn to="/Login">Login</v-btn>
                        <v-btn to="/AccountRegister">Register</v-btn>
                        <v-btn to="/Support">Support</v-btn>
                    </v-toolbar-items>
                </v-toolbar>
                <v-progress-linear :active="isLoading" :indeterminate="true" :height="10" color="orange"></v-progress-linear>
            </header>

            <main>
                <router-view></router-view>
            </main>
        </div>

        <v-footer tile class="inverseElevation" style="margin-top:70px;">
            <v-layout justify-center align-center text-center black--text font-weight-black>
                <v-flex>
                    <v-btn-toggle>
                        <v-btn x-large text to="/Help">Help</v-btn>
                        <v-btn x-large text to="/TermsOfService">Terms of Service</v-btn>
                        <v-btn x-large text to="/PrivacyPolicy">Privacy Policy</v-btn>
                    </v-btn-toggle>
                </v-flex>
                <v-flex xs1></v-flex>
                <v-flex>
                    &copy; 2018 - {{ new Date().getFullYear() }} Copyright: <a href="http://faucet4all.com"><strong>Faucet4all</strong></a> - All rights reserved.
                </v-flex>
            </v-layout>
        </v-footer>



        <success-modal></success-modal>
        <error-modal></error-modal>
        <captcha-modal></captcha-modal>
        <message-modal></message-modal>
        <chat-rules-modal></chat-rules-modal>
        <floating-square-banner left></floating-square-banner>
        <floating-square-banner right></floating-square-banner>

    </v-app>
</template>
<script>

    import psl from "psl";
    import { read } from 'fs';
    import { sync, get } from 'vuex-pathify';

    export default {
        data() {
            return {
                chatHistory: [],
                chatMessage: null,
                chatTaggingUsername: "@"
            };
        },
        sockets: {
            receivingUserData: function (data) {
                store.set('isLoading', true);

                store.set("userData", data["recordsets"][0][0]);
                store.set("loginHistory", data["recordsets"][1]);
                store.set("withdrawalHistory", data["recordsets"][2]);
                store.set("level1Referrals", data["recordsets"][3]);
                store.set("level2Referrals", data["recordsets"][4]);
                store.set("level3Referrals", data["recordsets"][5]);
                //store.set("storeFaucetSettings", data["recordsets"][6]);
                store.set("advertisePTP", data["recordsets"][7]);
                store.set("advertisePTPTimeBased", data["recordsets"][8]);
                store.set("advertiseBanner", data["recordsets"][9]);
                store.set("advertiseBannerTimeBased", data["recordsets"][10]);
                store.set("orderHistory", data["recordsets"][11]);
                store.set("depositHistory", data["recordsets"][12]);
                //Number 13 belongs to chat history and its code is after these
                store.set("ptpOrderHistory", data["recordsets"][14]);
                store.set("bannerOrderHistory", data["recordsets"][15]);
                store.set("squareBannerOrderHistory", data["recordsets"][16]);
                store.set("bonusAdOrderHistory", data["recordsets"][17]);
                store.set("advertiseBonusAd", data["recordsets"][18]);
                store.set("advertiseBonusAdTimeBased", data["recordsets"][19]);
                store.set("bonusAds", data["recordsets"][20]);

                this.chatTaggingUsername = "@" + store.get('userData.Username');
                this.chatHistory = data["recordsets"][13];

                store.set("authorization", 1);
                store.set("sessionValid", true);

                if (store.get('userData.JustClaimed') === true) {
                    this.displayMessage(
                        `You have successfully claimed ${store.get('faucetSettingValue', 'LinkClaimCredit')}`
                    );
                    this.$socket.emit(
                        "changeJustClaimed",
                        JSON.stringify({
                            sessionId: this.$cookie.get("sessionId"),
                            justClaimed: "false"
                        })
                    );
                }

                var userURL = this.$router.history.current.path;
                console.log(userURL);
                if (userURL === "/Login" || userURL === "/") {
                    this.$router.push("Home");
                }

                this.$nextTick(function () {
                    var element = this.$el.querySelector("#chatMessages");
                    element.scrollTop = element.scrollHeight;
                });

                store.set("isLoading", false);
            },
            receivingChatMessage: function (data) {
                this.chatHistory.push({
                    Username: data[0].Username,
                    DateTime: data[0].DateTime,
                    Message: data[0].Message
                });
                this.$nextTick(function () {
                    var element = this.$el.querySelector("#chatMessages");
                    element.scrollTop = element.scrollHeight;
                });
            },
            dataIsOutdated: function (data) {
                this.$socket.emit(
                    "requestingUserData",
                    JSON.stringify({ sessionId: this.$cookie.get("sessionId") })
                );
            }
        },
        methods: {
            openChatRulesModal: function () {
                store.set('chatRulesModal', true);
            },
            logOut: function () {
                this.$cookie.set("lsi", this.$cookie.get("sessionId"), {
                    expires: "30D"
                });
                this.$cookie.delete("sessionId");
                store.set("authorizationChange", 0);
                store.set("sessionValidState", false);
                store.set("storeUserData", null);
                this.$router.push("/");
            },
            sendChatMessage: function (data) {
                this.$validator.validate("chatMessage").then(result => {
                    if (result === true) {
                        var splittedMessage = this.chatMessage.split(" ");

                        var isLink = false;
                        var isInappropriateLanguage = false;

                        splittedMessage.forEach(function (element) {
                            var temporaryElement = element;
                            if (psl.isValid(temporaryElement.replace("http://", "")) === true)
                                isLink = true;
                            temporaryElement = element;
                            if (psl.isValid(temporaryElement.replace("https://", "")) === true)
                                isLink = true;
                        });

                        var splittedInappropriateLanguage = store.get('inappropriateWords').split("|");

                        splittedInappropriateLanguage.forEach(function (element) {
                            splittedMessage.forEach(function (subElement) {
                                if (element === subElement) isInappropriateLanguage = true;
                            });
                        });

                        if (isLink === true) {
                            this.$socket.emit(
                                "banChatUser",
                                JSON.stringify({ sessionId: this.$cookie.get("sessionId") })
                            );
                        } else if (isInappropriateLanguage === true) {
                            this.displayMessage(
                                "Please avoid using inappropriate language. Continuing the violation of our chat rules might get you banned from accessing the chat box."
                            );
                        } else {
                            this.$socket.emit(
                                "newChatMessage",
                                JSON.stringify({
                                    sessionId: this.$cookie.get("sessionId"),
                                    message: this.chatMessage
                                })
                            );
                        }
                        this.chatMessage = null;
                    }
                });
            },
            sendChatBannedMessage: function (data) {
                this.displayMessage(
                    "You have been banned from sending chat messages, it is due to the violation of our chat rules."
                );
            }
        },
        beforeMount() {
            this.executeInBackground(async () => {
                const response = await this.axios.get(process.env.VUE_APP_API_SETTINGS_Faucet);
                store.set('faucetSettings', response.data);
            }, false);
        },
        async mounted() {
            try {
                store.set("isLoading", true);
                store.set('referrer', this.$route.query.referrer);

                const response = await this.axios.get(process.env.VUE_APP_API_SessionValid, {
                    params: {
                        sessionId: this.$cookie.get("sessionId"),
                        lsi: this.$cookie.get("lsi")
                    }
                });
                this.$socket.emit(
                    "requestingUserData",
                    JSON.stringify({
                        sessionId: this.$cookie.get("sessionId")
                    }));
            }
            catch {
                store.set("authorization", 0);
                store.set("sessionValid", false);
            }
            finally {
                store.set("isLoading", false);
            }
        },
        computed: {
            ...get('*')
        }
    };
</script>
<style lang="scss">
    @import "../node_modules/ag-grid-community/dist/styles/ag-grid.css";
    @import "../node_modules/ag-grid-community/dist/styles/ag-theme-blue.css";
</style>
