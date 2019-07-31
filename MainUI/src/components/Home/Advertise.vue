<template>
    <div>
        <mdb-row>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col class="text-center">
                <mdb-row>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                </mdb-row>
                <h2 class="text-secondary">
                    Purchase Balance:
                    {{ this.$store.state.userData[0].PurchaseBalance.toFixed(8) }} BTC
                </h2>
                <span v-if="insufficientFunds" class="font-weight-bold text-danger">Insufficient funds in purchase balance.</span>
                <mdb-row>
                    <mdb-col>
                        <mdb-card border="secondary"
                                  class="mb-3 mx-auto"
                                  style="max-width: 18rem;">
                            <mdb-card-header class="text-secondary">PTP Traffic</mdb-card-header>
                            <mdb-card-body class="text-secondary">
                                <mdb-card-title tag="h5">Hits Based</mdb-card-title>
                                <mdb-card-text>
                                    <p class="text-secondary">1 Credit = 1 Hit</p>
                                    <v-select v-bind:options="ptpValues"
                                              v-model="ptpSelected"></v-select>
                                    <mdb-btn v-on:click="buyPTP()" color="secondary" size="sm">Buy</mdb-btn>
                                </mdb-card-text>
                            </mdb-card-body>
                        </mdb-card>
                    </mdb-col>
                    <mdb-col>
                        <mdb-card border="secondary"
                                  class="mb-3 mx-auto"
                                  style="max-width: 18rem;">
                            <mdb-card-header class="text-secondary">PTP Traffic</mdb-card-header>
                            <mdb-card-body class="text-secondary">
                                <mdb-card-title tag="h5">Time Based</mdb-card-title>
                                <mdb-card-text>
                                    <p class="text-secondary">1 Credit = 1 Day</p>
                                    <v-select v-bind:options="ptpTimeBasedValues"
                                              v-model="ptpTimeBasedSelected"></v-select>
                                    <mdb-btn v-on:click="buyPTPTimeBased()"
                                             color="secondary"
                                             size="sm">Buy</mdb-btn>
                                </mdb-card-text>
                            </mdb-card-body>
                        </mdb-card>
                    </mdb-col>
                </mdb-row>
                <mdb-row>
                    <mdb-col>
                        <mdb-card border="secondary"
                                  class="mb-3 mx-auto"
                                  style="max-width: 18rem;">
                            <mdb-card-header class="text-secondary">Banner Impressions</mdb-card-header>
                            <mdb-card-body class="text-secondary">
                                <mdb-card-title tag="h5">Hits Based</mdb-card-title>
                                <mdb-card-text>
                                    <p class="text-secondary">1 Credit = 1 Impression</p>
                                    <v-select v-bind:options="bannerValues"
                                              v-model="bannerSelected"></v-select>
                                    <mdb-btn v-on:click="buyBanner()" color="secondary" size="sm">Buy</mdb-btn>
                                </mdb-card-text>
                            </mdb-card-body>
                        </mdb-card>
                    </mdb-col>
                    <mdb-col>
                        <mdb-card border="secondary"
                                  class="mb-3 mx-auto"
                                  style="max-width: 18rem;">
                            <mdb-card-header class="text-secondary">Banner Impressions</mdb-card-header>
                            <mdb-card-body class="text-secondary">
                                <mdb-card-title tag="h5">Time Based</mdb-card-title>
                                <mdb-card-text>
                                    <p class="text-secondary">1 Credit = 1 Day</p>
                                    <v-select v-bind:options="bannerTimeBasedValues"
                                              v-model="bannerTimeBasedSelected"></v-select>
                                    <mdb-btn v-on:click="buyBannerTimeBased()"
                                             color="secondary"
                                             size="sm">Buy</mdb-btn>
                                </mdb-card-text>
                            </mdb-card-body>
                        </mdb-card>
                    </mdb-col>
                </mdb-row>
                <mdb-row>
                    <mdb-col>
                        <mdb-card border="secondary"
                                  class="mb-3 mx-auto"
                                  style="max-width: 18rem;">
                            <mdb-card-header class="text-secondary">Bonus Ad Hits</mdb-card-header>
                            <mdb-card-body class="text-secondary">
                                <mdb-card-title tag="h5">Hits Based</mdb-card-title>
                                <mdb-card-text>
                                    <p class="text-secondary">1 Credit = 1 Impression</p>
                                    <v-select v-bind:options="bonusAdValues"
                                              v-model="bonusAdSelected"></v-select>
                                    <mdb-btn v-on:click="buyBonusAd()" color="secondary" size="sm">Buy</mdb-btn>
                                </mdb-card-text>
                            </mdb-card-body>
                        </mdb-card>
                    </mdb-col>
                    <mdb-col>
                        <mdb-card border="secondary"
                                  class="mb-3 mx-auto"
                                  style="max-width: 18rem;">
                            <mdb-card-header class="text-secondary">Bonus Ad Hits</mdb-card-header>
                            <mdb-card-body class="text-secondary">
                                <mdb-card-title tag="h5">Time Based</mdb-card-title>
                                <mdb-card-text>
                                    <p class="text-secondary">1 Credit = 1 Day</p>
                                    <v-select v-bind:options="bonusAdTimeBasedValues"
                                              v-model="bonusAdTimeBasedSelected"></v-select>
                                    <mdb-btn v-on:click="buyBonusAdTimeBased()"
                                             color="secondary"
                                             size="sm">Buy</mdb-btn>
                                </mdb-card-text>
                            </mdb-card-body>
                        </mdb-card>
                    </mdb-col>
                </mdb-row>
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
                <standard-banner></standard-banner><br />
            </mdb-col>
            <mdb-col>
                <standard-banner></standard-banner><br />
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
        mdbCard,
        mdbCardHeader,
        mdbCardBody,
        mdbCardTitle,
        mdbCardText
    } from "mdbvue";
    export default {
        components: {
            mdbRow,
            mdbCol,
            mdbBtn,
            mdbCard,
            mdbCardBody,
            mdbCardTitle,
            mdbCardText,
            mdbCardHeader
        },
        data()
        {
            return {
                recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
                recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
                address: this.$store.state.userData[0].FaucetHubBitcoinAddress,
                apiResponse: null,
                errorList: null,
                successList: null,
                value: null,
                price: null,
                bonusAdValues: [],
                bonusAdTimeBasedValues: [],
                bannerValues: [],
                bannerTimeBasedValues: [],
                ptpValues: [],
                ptpTimeBasedValues: [],
                bannerSelected: null,
                bannerTimeBasedSelected: null,
                bonusAdTimeBasedSelected: null,
                bonusAdSelected: null,
                ptpSelected: null,
                ptpTimeBasedSelected: null,
                insufficientFunds: false
            };
        },
        methods: {
            test: function () { },
            buyPTP: async function ()
            {
                try
                {
                    if (this.ptpSelected === null)
                        return;
                    if (this.ptpSelected["price"] > this.$store.state.userData[0].PurchaseBalance)
                        this.insufficientFunds = true;
                    else
                    {
                        await this.axios.patch(process.env.VUE_APP_API_Advertise, { sessionId: this.$cookie.get("sessionId"), reference: this.ptpSelected["value"] });
                        this.$store.commit("successMessage");
                        this.$store.commit("successModal", true);
                        this.addPTPData();
                        this.$socket.emit("requestingUserData", JSON.stringify({ sessionId: this.$cookie.get("sessionId") }));
                    }
                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.response.data);
                    this.$store.commit("errorModal", true);
                }
            },
            buyPTPTimeBased: async function ()
            {
                try
                {
                    if (this.ptpTimeBasedSelected === null)
                        return;
                    if (this.ptpTimeBasedSelected["price"] > this.$store.state.userData[0].PurchaseBalance)
                        this.insufficientFunds = true;
                    else
                    {
                        await this.axios.patch(process.env.VUE_APP_API_Advertise, { sessionId: this.$cookie.get("sessionId"), reference: this.ptpTimeBasedSelected["value"] });
                        this.$store.commit("successModal", true);
                        this.addPTPTimeBasedData();
                        this.$socket.emit("requestingUserData", JSON.stringify({ sessionId: this.$cookie.get("sessionId") }));
                    }

                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }
            },
            buyBanner: async function ()
            {

                try
                {
                    if (this.bannerSelected === null)
                        return;
                    if (this.bannerSelected["price"] > this.$store.state.userData[0].PurchaseBalance)
                        this.insufficientFunds = true;
                    else
                    {
                        await this.axios.patch(process.env.VUE_APP_API_Advertise, { sessionId: this.$cookie.get("sessionId"), reference: this.bannerSelected["value"] });
                        this.$store.commit("successModal", true);
                        this.addBannerData();
                        this.$socket.emit("requestingUserData", JSON.stringify({ sessionId: this.$cookie.get("sessionId") }));

                    }


                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }

            },
            buyBannerTimeBased: async function ()
            {

                try
                {
                    if (this.bannerTimeBasedSelected === null)
                        return;
                    if (this.bannerTimeBasedSelected["price"] > this.$store.state.userData[0].PurchaseBalance)

                        this.insufficientFunds = true;
                    else
                    {
                        await this.axios.patch(process.env.VUE_APP_API_Advertise, { sessionId: this.$cookie.get("sessionId"), reference: this.bannerTimeBasedSelected["value"] });
                        this.$store.commit("successModal", true);
                        this.addBannerTimeBasedData();
                        this.$socket.emit("requestingUserData", JSON.stringify({ sessionId: this.$cookie.get("sessionId") }));

                    }

                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }


            },
            buyBonusAd: async function ()
            {

                try
                {
                    if (this.bonusAdSelected === null)
                        return;
                    if (this.bonusAdSelected["price"] > this.$store.state.userData[0].PurchaseBalance)

                        this.insufficientFunds = true;
                    else
                    {
                        await this.axios.patch(process.env.VUE_APP_API_Advertise, { sessionId: this.$cookie.get("sessionId"), reference: this.bonusAdSelected["value"] });
                        this.$store.commit("successModal", true);
                        this.addBonusAdData();
                        this.$socket.emit("requestingUserData", JSON.stringify({ sessionId: this.$cookie.get("sessionId") }));

                    }

                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }


            },
            buyBonusAdTimeBased: async function ()
            {

                try
                {
                    if (this.bonusAdTimeBasedSelected === null)
                        return;
                    if (this.bonusAdTimeBasedSelected["price"] > this.$store.state.userData[0].PurchaseBalance)

                        this.insufficientFunds = true;
                    else
                    {
                        await this.axios.patch(process.env.VUE_APP_API_Advertise, { sessionId: this.$cookie.get("sessionId"), reference: this.bonusAdTimeBasedSelected["value"] });
                        this.$store.commit("successModal", true);
                        this.addBonusAdTimeBasedData();
                        this.$socket.emit("requestingUserData", JSON.stringify({ sessionId: this.$cookie.get("sessionId") }));

                    }


                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }

            },
            addPTPData: async function ()
            {
                try
                {
                    var response = await this.axios.get(process.env.VUE_APP_API_Advertise, { params: { sessionId: this.$cookie.get("sessionId"), type: 'PTP', isTimeBased: false } });
                    var data = response.data;
                    Object.entries(data).forEach(([key, value]) => this.ptpValues.push({
                        label: `${data[key]['Credit']} Credits - ${data[key]['Price'].toFixed(8)} BTC`,
                        value: data[key]['Reference'],
                        price: data[key]['Price'].toFixed(8)
                    }));
                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }
            },
            addPTPTimeBasedData: async function ()
            {
                try
                {
                    var response = await this.axios.get(process.env.VUE_APP_API_Advertise, { params: { sessionId: this.$cookie.get("sessionId"), type: 'PTP', isTimeBased: true } });
                    var data = response.data;
                    Object.entries(data).forEach(([key, value]) => this.ptpTimeBasedValues.push({
                        label: `${data[key]['Credit']} Credits - ${data[key]['Price'].toFixed(8)} BTC`,
                        value: data[key]['Reference'],
                        price: data[key]['Price'].toFixed(8)
                    }));
                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }
            },
            addBannerData: async function ()
            {
                try
                {
                    var response = await this.axios.get(process.env.VUE_APP_API_Advertise, { params: { sessionId: this.$cookie.get("sessionId"), type: 'Banner', isTimeBased: false } });
                    var data = response.data;
                    Object.entries(data).forEach(([key, value]) => this.bannerValues.push({
                        label: `${data[key]['Credit']} Credits - ${data[key]['Price'].toFixed(8)} BTC`,
                        value: data[key]['Reference'],
                        price: data[key]['Price'].toFixed(8)
                    }));
                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }
            },
            addBannerTimeBasedData: async function ()
            {
                try
                {
                    var response = await this.axios.get(process.env.VUE_APP_API_Advertise, { params: { sessionId: this.$cookie.get("sessionId"), type: 'Banner', isTimeBased: true } });
                    var data = response.data;
                    Object.entries(data).forEach(([key, value]) => this.bannerTimeBasedValues.push({
                        label: `${data[key]['Credit']} Credits - ${data[key]['Price'].toFixed(8)} BTC`,
                        value: data[key]['Reference'],
                        price: data[key]['Price'].toFixed(8)
                    }));
                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }
            },
            addBonusAdData: async function ()
            {
                try
                {
                    var response = await this.axios.get(process.env.VUE_APP_API_Advertise, { params: { sessionId: this.$cookie.get("sessionId"), type: 'BonusAd', isTimeBased: false } });
                    var data = response.data;
                    Object.entries(data).forEach(([key, value]) => this.bonusAdValues.push({
                        label: `${data[key]['Credit']} Credits - ${data[key]['Price'].toFixed(8)} BTC`,
                        value: data[key]['Reference'],
                        price: data[key]['Price'].toFixed(8)
                    }));
                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }
            },
            addBonusAdTimeBasedData: async function ()
            {
                try
                {
                    var response = await this.axios.get(process.env.VUE_APP_API_Advertise, { params: { sessionId: this.$cookie.get("sessionId"), type: 'BonusAd', isTimeBased: true } });
                    var data = response.data;
                    Object.entries(data).forEach(([key, value]) => this.bonusAdTimeBasedValues.push({
                        label: `${data[key]['Credit']} Credits - ${data[key]['Price'].toFixed(8)} BTC`,
                        value: data[key]['Reference'],
                        price: data[key]['Price'].toFixed(8)
                    }));
                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }
            },
            addAllData: function ()
            {
                try
                {
                    this.addPTPData();
                    this.addBannerData();
                    this.addBonusAdData();
                    this.addPTPTimeBasedData();
                    this.addBannerTimeBasedData();
                    this.addBonusAdTimeBasedData();
                }
                catch (error)
                {
                    this.$store.commit("errorMessage", error.toString());
                    this.$store.commit("errorModal", true);
                }
            }
        },
        mounted()
        {
            try
            {
                this.addAllData();
            }
            catch (error)
            {
                this.$store.commit("errorMessage", error.toString());
                this.$store.commit("errorModal", true);
            }
        }
    };
</script>
