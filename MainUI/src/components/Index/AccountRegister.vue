<template>
    <v-container fluid text-center justify-center align-center align-content-center pa-0>
        <br />
        <v-layout>
            <v-flex>
                <standard-banner></standard-banner>
                <br />
                <standard-banner></standard-banner>
                <br />

                <v-layout>
                    <v-flex>
                        <skyscrapper-banner></skyscrapper-banner>
                    </v-flex>
                    <v-flex>
                        <skyscrapper-banner></skyscrapper-banner>
                    </v-flex>
                </v-layout>

            </v-flex>

            <v-flex>
                <!--Registeration form-->
                <v-card elevation="24" style="margin-top: 10px;" class="font-weight-black black--text">
                    <v-card-title>
                        <p class="ma-auto">Account Register</p>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text>
                        <v-layout>
                            <v-flex xs2 class="align-self-center">
                                <v-icon x-large class="black--text">mdi-account</v-icon>
                            </v-flex>
                            <v-flex>
                                <v-text-field label="Username"
                                              filled
                                              name="username"
                                              autofocus
                                              v-validate="'required|min:3|max:10|alpha_num'"
                                              v-model.trim="username"
                                              :error-messages="errors.collect('username')"
                                              :counter="10"></v-text-field>
                            </v-flex>
                        </v-layout>
                        <v-layout>
                            <v-flex xs2 class="align-self-center">
                                <v-icon x-large class="black--text">mdi-email</v-icon>
                            </v-flex>
                            <v-flex>
                                <v-text-field label="Email"
                                              filled
                                              name="email"
                                              type="email"
                                              v-validate="'required|email|min:3|max:350'"
                                              v-model.trim="email"
                                              :error-messages="errors.collect('email')"
                                              :counter="350"></v-text-field>
                            </v-flex>
                        </v-layout>

                        <!--v-model is need for both password fields, do not include v-dalidate-as, ref tag is used as a marker to help v-validate find password matching field-->
                        <v-layout>
                            <v-flex xs2 class="align-self-center">
                                <v-icon x-large class="black--text">mdi-lock</v-icon>
                            </v-flex>
                            <v-flex>
                                <v-text-field label="Password"
                                              filled
                                              name="password"
                                              type="password"
                                              v-validate="'required|min:10|max:128'"
                                              v-model.trim="password"
                                              ref="password"
                                              :error-messages="errors.collect('password')"
                                              :counter="128"></v-text-field>
                            </v-flex>
                        </v-layout>

                        <v-layout>
                            <v-flex xs2 class="align-self-center">
                                <v-icon x-large class="black--text">mdi-alert</v-icon>
                            </v-flex>
                            <v-flex>
                                <v-text-field label="Confirm Password"
                                              filled
                                              name="confirmPassword"
                                              type="password"
                                              v-validate="'required|min:10|max:128|confirmed:password'"
                                              v-model.trim="confirmPassword"
                                              :error-messages="errors.collect('confirmPassword')"
                                              :counter="128"></v-text-field>
                            </v-flex>
                        </v-layout>

                        <div id="recaptcha-main"
                             class="g-recaptcha"
                             :data-sitekey="[recaptchav2SiteKey]"
                             style="display: inline-block;"></div>
                        <br />
                        <a href="#/ResendEmail" class="font-weight-bold">Didn't receive activation email ?</a>
                    </v-card-text>
                    <v-divider></v-divider>
                    <v-card-actions>
                        <v-container fluid text-center justify-center align-baseline pa-0>
                            <v-layout>
                                <v-flex>
                                    <v-btn ripple
                                           outlined
                                           x-large
                                           color="orange"
                                           @click="sendAccountRegisterApi()"
                                           class="animated infinite rubberBand">
                                        <v-icon x-large>mdi-cube-send</v-icon>
                                        Register
                                    </v-btn>
                                </v-flex>
                            </v-layout>
                        </v-container>
                    </v-card-actions>
                </v-card>
            </v-flex>

            <v-flex>
                <standard-banner></standard-banner>
                <br />
                <standard-banner></standard-banner>
                <br />
                <v-layout>
                    <v-flex>
                        <skyscrapper-banner></skyscrapper-banner>
                    </v-flex>
                    <v-flex>
                        <skyscrapper-banner></skyscrapper-banner>
                    </v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
    </v-container>
</template>

<script>

    export default {
        data() {
            return {
                recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
                username: null,
                password: null,
                confirmPassword: null,
                email: null,
                ip: "0.0.0.0"
            };
        },
        mounted() {

            try {

                this.$nextTick(function () { grecaptcha.render("recaptcha-main"); });
            }
            catch (error) {
                console.log(error);
            }
        },
        async updated() {

            try {

                const response = await this.axios.get(process.env.VUE_APP_API_IP);
                this.ip = response.data.ip;

            }
            catch (error) {
                if (error.isAxiosError) {
                    var concatMessages = '';
                    Object.entries(error.response.data.errors).forEach(([key, value]) => concatMessages += value);
                    this.$store.commit("errorMessage", concatMessages);
                    this.$store.commit("errorModal", true);
                }
                else {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }
            }

        },
        methods: {

            sendAccountRegisterApi: async function () {

                try {
                    this.$store.commit("isLoading", true);

                    const result = await this.$validator.validateAll();
                    if (!result) return;

                    const response = await this.axios
                        .post(process.env.VUE_APP_API_AccountRegister, {
                            username: this.username,
                            email: this.email,
                            password: this.password,
                            confirmPassword: this.confirmPassword,
                            referrer: this.$cookie.get("referrer"),
                            recaptchav2Response: grecaptcha.getResponse(),
                            ip: this.ip
                        });

                    this.$store.commit("successMessage", response.data.message);
                    this.$store.commit("successModal", true);

                }
                catch (error) {

                    if (error.isAxiosError) {
                        var concatMessages = '';
                        Object.entries(error.response.data.errors).forEach(([key, value]) => concatMessages += value);
                        this.$store.commit("errorMessage", concatMessages);
                        this.$store.commit("errorModal", true);
                    }
                    else {
                        this.$store.commit("errorMessage", error.toString());
                        this.$store.commit("errorModal", true);
                    }
                }
                finally {
                    grecaptcha.reset();
                    this.$store.commit("isLoading", false);
                }
            }
        }
    };
</script>
