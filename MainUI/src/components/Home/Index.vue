<template>
    <div>
        <mdb-row>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col>
                <mdb-row>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                </mdb-row>

                <p class="font-weight-bold">Claim</p>

                <p class="font-weight-bold">
                    We pay you for 3 levels of referrals deep. <br />
                    We pay {{ this.$store.getters.faucetSettingValue("Level1").Value }}%
                    to you from all your Level 1 earnings,<br />
                    {{ this.$store.getters.faucetSettingValue("Level2").Value }}% from all
                    your level 2 referrals earnings,<br />
                    {{ this.$store.getters.faucetSettingValue("Level3").Value }}% to you
                    from all your level 3 referrals earnings
                </p>

                <div id="adcopy-outer" class="text-secondary w-50 mx-auto">
                    <mdb-card border="secondary">
                        <mdb-card-body>
                            <div id="adcopy-puzzle-image" class="mx-auto"></div>
                        </mdb-card-body>
                        <mdb-card-footer border="secondary">
                            <div><span id="adcopy-instr"></span></div>
                            <input type="text" name="adcopy_response" id="adcopy_response" />
                            <input type="hidden"
                                   name="adcopy_challenge"
                                   id="adcopy_challenge" />
                            <mdb-btn onclick="javascript: ACPuzzle.reload()"
                                     color="secondary"
                                     size="sm"
                                     id="adcopy-link-refresh">
                                <mdb-icon icon="redo-alt" />
                            </mdb-btn>
                        </mdb-card-footer>
                    </mdb-card>
                    <div><span id="adcopy-error-msg"></span></div>
                    <div id="adcopy-puzzle-audio"></div>
                    <div id="adcopy-pixel-image"></div>
                    <a href="javascript:ACPuzzle.change2audio()"
                       id="adcopy-link-audio"></a>
                    <a href="javascript:ACPuzzle.change2image()"
                       id="adcopy-link-image"></a>
                    <a href="javascript:ACPuzzle.moreinfo()" id="adcopy-link-info"></a>
                </div>

                <div id="solveMedia"></div>
                <script2>
                    var ACPuzzleOptions = { tabindex: 1, theme: 'custom', lang: 'en',
                    size: '300x150' }; ACPuzzle.create('7.ik62YmiFKx8NNhAaP9wwTk85XuAjuP',
                    'solveMedia', ACPuzzleOptions);
                </script2>

                <p class="font-weight-bold">
                    You need to wait {{ remainingSeconds }} seconds before claiming again.
                </p>

                <div v-html="PPCStandardBanner1"></div>
                <br />

                <div class="text-center mt-4">
                    <mdb-btn v-on:click="requestClaimLink()" color="secondary">Claim</mdb-btn>
                </div>
            </mdb-col>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col col="1"> </mdb-col>
        </mdb-row>

        <mdb-row>
            <mdb-col>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </mdb-col>
            <mdb-col>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </mdb-col>
        </mdb-row>

        <button type="button"
                class="bottomLeftButton"
                id="bottomLeftButton"
                onClick="document.getElementById('bottomLeftBanner').remove();this.remove();">
            x
        </button>
        <div class="bottomLeftBanner" id="bottomLeftBanner">
            <square-banner></square-banner>
        </div>

        <button type="button"
                class="bottomRightButton"
                id="bottomRightButton"
                onClick="document.getElementById('bottomRightBanner').remove();this.remove();">
            x
        </button>
        <div class="bottomRightBanner" id="bottomRightBanner">
            <square-banner></square-banner>
        </div>
    </div>
</template>

<script>
    import
    {
        mdbRow,
        mdbCol,
        mdbBtn,
        mdbIcon,
        mdbInput,
        mdbCard,
        mdbCardHeader,
        mdbCardFooter,
        mdbCardBody,
        mdbCardTitle,
        mdbCardText
    } from "mdbvue";

    export default {
        components: {
            mdbRow,
            mdbCol,
            mdbBtn,
            mdbIcon,
            mdbInput,
            mdbCard,
            mdbCardHeader,
            mdbCardFooter,
            mdbCardBody,
            mdbCardTitle,
            mdbCardText
        },
        data()
        {
            return {
                recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
                recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
                testVal: null,
                claimLink: null,
                remainingSeconds: 0,
                justClaimed: false,
                PPCStandardBanner1: this.$store.getters.randomPPCBannerNetworkHTMLCode,
                ip: "0.0.0.0"
            };
        },
        sockets: {
            receivingClaimLink: function (data)
            {
                //var confirmationCodeVariable = data[0]["ConfirmationCode"];
                //Remove the leading dot if you want to test in localhost
                //this.$cookie.set('confirmationCode', confirmationCodeVariable, { domain: 'localhost' });

                this.$store.commit("isLoading", false);
                window.location.href = "http://localhost:3005";
            },
            receivingTestSocket: function (data)
            {
                this.testVal = data;
            }
        },
        methods: {
            test: function (url)
            {
                var hostname;
                //find & remove protocol (http, ftp, etc.) and get hostname

                if (url.indexOf("//") > -1)
                {
                    hostname = url.split("/")[2];
                } else
                {
                    hostname = url.split("/")[0];
                }

                //find & remove port number
                hostname = hostname.split(":")[0];
                //find & remove "?"
                hostname = hostname.split("?")[0];

                return hostname;
            },
            requestClaimLink: function ()
            {
                if (this.remainingSeconds === 0)
                {
                    this.$store.commit("isLoading", true);
                    this.$socket.emit(
                        "requestingClaimLink",
                        JSON.stringify({
                            sessionId: this.$cookie.get("sessionId"),
                            challenge: document.getElementById("adcopy_challenge").value,
                            response: document.getElementById("adcopy_response").value,
                            ip: this.ip
                        })
                    );
                }
            },
            startCountDown: function ()
            {
                setInterval(this.decrementCountDown, 1000); //Remember not to add () for this function call here otherwise it will not loop
            },
            decrementCountDown: function ()
            {
                if (this.remainingSeconds > 0)
                {
                    this.remainingSeconds = this.remainingSeconds - 1;
                }
            }
        },
        mounted()
        {
            this.$store.commit("isLoading", true);

            this.axios
                .get(process.env.VUE_APP_API_IP)
                .then(response =>
                {
                    this.ip = response.data.ip;
                })
                .catch(error => { });

            //var gmtZone = moment().utcOffset() / 60;
            var gmtZone = 5;
            var dateTimeNow = this.moment().add(gmtZone, "hours");
            var dateTimeFuture = this.moment(
                this.$store.state.userData[0].LinkClaimInterval
            );
            var resultantSeconds = dateTimeFuture.diff(dateTimeNow, "seconds");

            if (resultantSeconds > 0)
            {
                this.remainingSeconds = resultantSeconds;
                this.startCountDown();
            }

            this.$store.commit("isLoading", false);
        }
    };
</script>
