<template>
    <div>
        <mdb-row>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
                <br />
                <skyscrapper-banner></skyscrapper-banner>
                <br />
                <skyscrapper-banner></skyscrapper-banner>
                <br />
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col>
                <mdb-row>
                    <mdb-col class="text-center">
                        <standard-banner></standard-banner><br />
                    </mdb-col>
                    <mdb-col class="text-center">
                        <standard-banner></standard-banner><br />
                    </mdb-col>
                </mdb-row>

                <mdb-row>
                    <mdb-col col="1"></mdb-col>
                    <mdb-col class="text-center">
                        <!--Self-->
                        <mdb-tbl striped bordered hover small>
                            <mdb-tbl-head color="secondary-color text-white">
                                <tr>
                                    <th colspan="2">Account Details</th>
                                </tr>
                            </mdb-tbl-head>

                            <mdb-tbl-body>
                                <tr>
                                    <th>Username</th>
                                    <td>{{ userData[0].Username }}</td>
                                </tr>
                                <tr>
                                    <th>Balance</th>
                                    <td>
                                        {{ userData[0].Balance.toFixed(8) }}
                                        <mdb-btn color="secondary"
                                                 size="sm"
                                                 v-on:click="routerPush('Withdrawal')">Withdraw</mdb-btn>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Offerwall Balance</th>
                                    <td>
                                        {{ userData[0].OfferwallBalance.toFixed(8) }}
                                        <mdb-btn color="secondary"
                                                 size="sm"
                                                 v-on:click="routerPush('Withdrawal')">Withdraw</mdb-btn>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Purchase Balance</th>
                                    <td>{{ userData[0].PurchaseBalance.toFixed(8) }}</td>
                                </tr>
                                <tr>
                                    <th>Date Registered</th>
                                    <td>
                                        {{
                      this.moment(userData[0].DateRegistered).format(
                        "YYYY-MM-DD - hh:mm:ss A"
                      )
                                        }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Referrer</th>
                                    <td>{{ userData[0].Referrer }}</td>
                                </tr>
                                <tr>
                                    <th>Total Withdrawn</th>
                                    <td>{{ userData[0].TotalWithdrawn.toFixed(8) }}</td>
                                </tr>
                                <tr>
                                    <th>Total Offerwall Withdrawn</th>
                                    <td>{{ userData[0].TotalOfferwallWithdrawn.toFixed(8) }}</td>
                                </tr>
                                <tr>
                                    <th>Faucet Claim Earning</th>
                                    <td>{{ userData[0].LinkShortnerEarning.toFixed(8) }}</td>
                                </tr>
                                <tr>
                                    <th>Offerwall Earning</th>
                                    <td>{{ userData[0].OfferwallEarning.toFixed(8) }}</td>
                                </tr>
                                <tr>
                                    <th>PTP Unique Earning</th>
                                    <td>{{ userData[0].PTPUniqueEarning.toFixed(8) }}</td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 1 Earning</th>
                                    <td>{{ userData[0].PTPNonUnique1Earning.toFixed(8) }}</td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 2 Earning</th>
                                    <td>{{ userData[0].PTPNonUnique2Earning.toFixed(8) }}</td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 3 Earning</th>
                                    <td>{{ userData[0].PTPNonUnique3Earning.toFixed(8) }}</td>
                                </tr>
                                <tr>
                                    <th>Level 1 Referrals</th>
                                    <td>{{ level1Referrals.length }}</td>
                                </tr>
                                <tr>
                                    <th>Level 2 Referrals</th>
                                    <td>{{ level2Referrals.length }}</td>
                                </tr>
                                <tr>
                                    <th>Level 3 Referrals</th>
                                    <td>{{ level3Referrals.length }}</td>
                                </tr>
                            </mdb-tbl-body>
                        </mdb-tbl>
                        <!--Advertising Balance-->
                        <mdb-tbl striped bordered hover small>
                            <mdb-tbl-head color="secondary-color text-white">
                                <tr>
                                    <th colspan="2">Advertising Details</th>
                                </tr>
                            </mdb-tbl-head>

                            <mdb-tbl-body>
                                <tr>
                                    <th>PTP Credit</th>
                                    <td>{{ userData[0].PTPCredit }}</td>
                                </tr>
                                <tr>
                                    <th>PTP Day Credit</th>
                                    <td>{{ userData[0].PTPDayCredit }}</td>
                                </tr>
                                <tr>
                                    <th>Banner Credit</th>
                                    <td>{{ userData[0].BannerCredit }}</td>
                                </tr>
                                <tr>
                                    <th>Banner Day Credit</th>
                                    <td>{{ userData[0].BannerDayCredit }}</td>
                                </tr>
                                <tr>
                                    <th>Bonus Ad Credit</th>
                                    <td>{{ userData[0].BonusAdCredit }}</td>
                                </tr>
                                <tr>
                                    <th>Bonus Ad Day Credit</th>
                                    <td>{{ userData[0].BonusAdDayCredit }}</td>
                                </tr>
                            </mdb-tbl-body>
                        </mdb-tbl>
                        <!--Level 1-->
                        <mdb-tbl striped bordered hover small>
                            <mdb-tbl-head color="secondary-color text-white">
                                <tr>
                                    <th colspan="2">Level 1 Referrals Details</th>
                                </tr>
                            </mdb-tbl-head>

                            <mdb-tbl-body>
                                <tr>
                                    <th>Total Referrals</th>
                                    <td>{{ level1Referrals.length }}</td>
                                </tr>
                                <tr>
                                    <th>
                                        Total Credited ({{ level1ReferralsPercentageShare }}%
                                        Earning)
                                    </th>
                                    <td>
                                        {{ parseFloat(level1ReferralsTotalCredited).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Faucet Claim Earning</th>
                                    <td>
                                        {{
                      parseFloat(level1ReferralsFaucetClaimEarning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Bonus Ad Earning</th>
                                    <td>
                                        {{ parseFloat(level1ReferralsBonusAdEarning).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Offerwall Earning</th>
                                    <td>
                                        {{ parseFloat(level1ReferralsOfferwallEarning).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Unique Earning</th>
                                    <td>
                                        {{ parseFloat(level1ReferralsPTPUniqueEarning).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 1 Earning</th>
                                    <td>
                                        {{
                      parseFloat(level1ReferralsPTPNonUnique1Earning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 2 Earning</th>
                                    <td>
                                        {{
                      parseFloat(level1ReferralsPTPNonUnique2Earning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 3 Earning</th>
                                    <td>
                                        {{
                      parseFloat(level1ReferralsPTPNonUnique3Earning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                            </mdb-tbl-body>
                        </mdb-tbl>
                        <!--Level 2-->
                        <mdb-tbl striped bordered hover small>
                            <mdb-tbl-head color="secondary-color text-white">
                                <tr>
                                    <th colspan="2">Level 2 Referrals Details</th>
                                </tr>
                            </mdb-tbl-head>

                            <mdb-tbl-body>
                                <tr>
                                    <th>Total Referrals</th>
                                    <td>{{ level2Referrals.length }}</td>
                                </tr>
                                <tr>
                                    <th>
                                        Total Credited ({{ level2ReferralsPercentageShare }}%
                                        Earning)
                                    </th>
                                    <td>
                                        {{ parseFloat(level2ReferralsTotalCredited).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Faucet Claim Earning</th>
                                    <td>
                                        {{
                      parseFloat(level2ReferralsFaucetClaimEarning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Bonus Ad Earning</th>
                                    <td>
                                        {{ parseFloat(level2ReferralsBonusAdEarning).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Offerwall Earning</th>
                                    <td>
                                        {{ parseFloat(level2ReferralsOfferwallEarning).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Unique Earning</th>
                                    <td>
                                        {{ parseFloat(level2ReferralsPTPUniqueEarning).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 1 Earning</th>
                                    <td>
                                        {{
                      parseFloat(level2ReferralsPTPNonUnique2Earning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 2 Earning</th>
                                    <td>
                                        {{
                      parseFloat(level2ReferralsPTPNonUnique2Earning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 3 Earning</th>
                                    <td>
                                        {{
                      parseFloat(level2ReferralsPTPNonUnique3Earning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                            </mdb-tbl-body>
                        </mdb-tbl>
                        <!--Level 3-->
                        <mdb-tbl striped bordered hover small>
                            <mdb-tbl-head color="secondary-color text-white">
                                <tr>
                                    <th colspan="2">Level 3 Referrals Details</th>
                                </tr>
                            </mdb-tbl-head>

                            <mdb-tbl-body>
                                <tr>
                                    <th>Total Referrals</th>
                                    <td>{{ level3Referrals.length }}</td>
                                </tr>
                                <tr>
                                    <th>
                                        Total Credited ({{ level3ReferralsPercentageShare }}%
                                        Earning)
                                    </th>
                                    <td>
                                        {{ parseFloat(level3ReferralsTotalCredited).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Faucet Claim Earning</th>
                                    <td>
                                        {{
                      parseFloat(level3ReferralsFaucetClaimEarning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Bonus Ad Earning</th>
                                    <td>
                                        {{ parseFloat(level3ReferralsBonusAdEarning).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>Offerwall Earning</th>
                                    <td>
                                        {{ parseFloat(level3ReferralsOfferwallEarning).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Unique Earning</th>
                                    <td>
                                        {{ parseFloat(level3ReferralsPTPUniqueEarning).toFixed(8) }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 1 Earning</th>
                                    <td>
                                        {{
                      parseFloat(level3ReferralsPTPNonUnique3Earning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                                <tr>
                                    <th>PTP Non-Unique 3 Earning</th>
                                    <td>
                                        {{
                      parseFloat(level3ReferralsPTPNonUnique3Earning).toFixed(8)
                                        }}
                                    </td>
                                </tr>
                            </mdb-tbl-body>
                        </mdb-tbl>
                    </mdb-col>
                    <mdb-col col="1"></mdb-col>

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
                </mdb-row>
            </mdb-col>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
                <br />
                <skyscrapper-banner></skyscrapper-banner>
                <br />
                <skyscrapper-banner></skyscrapper-banner>
                <br />
                <skyscrapper-banner></skyscrapper-banner>
                <br />
            </mdb-col>
            <mdb-col col="1"> </mdb-col>
        </mdb-row>
    </div>
</template>

<script>
    import
    {
        mdbRow,
        mdbCol,
        Btn,
        mdbTbl,
        mdbTblHead,
        mdbTblBody,
        mdbBtn
    } from "mdbvue";

    export default {
        components: { mdbRow, mdbCol, Btn, mdbTbl, mdbTblHead, mdbTblBody, mdbBtn },
        data()
        {
            return {
                recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
                recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
                userData: this.$store.state.userData,

                level1Referrals: this.$store.state.level1Referrals,
                level1ReferralsPercentageShare: this.$store.getters.faucetSettingValue(
                    "Level1"
                ).Value,
                level1ReferralsTotalCredited: 0,
                level1ReferralsFaucetClaimEarning: 0,
                level1ReferralsBonusAdEarning: 0,
                level1ReferralsPTPUniqueEarning: 0,
                level1ReferralsPTPNonUnique1Earning: 0,
                level1ReferralsPTPNonUnique2Earning: 0,
                level1ReferralsPTPNonUnique3Earning: 0,
                level1ReferralsOfferwallEarning: 0,

                level2Referrals: this.$store.state.level2Referrals,
                level2ReferralsPercentageShare: this.$store.getters.faucetSettingValue(
                    "Level2"
                ).Value,
                level2ReferralsTotalCredited: 0,
                level2ReferralsFaucetClaimEarning: 0,
                level2ReferralsBonusAdEarning: 0,
                level2ReferralsPTPUniqueEarning: 0,
                level2ReferralsPTPNonUnique2Earning: 0,
                level2ReferralsPTPNonUnique2Earning: 0,
                level2ReferralsPTPNonUnique3Earning: 0,
                level2ReferralsOfferwallEarning: 0,

                level3Referrals: this.$store.state.level3Referrals,
                level3ReferralsPercentageShare: this.$store.getters.faucetSettingValue(
                    "Level3"
                ).Value,
                level3ReferralsTotalCredited: 0,
                level3ReferralsFaucetClaimEarning: 0,
                level3ReferralsBonusAdEarning: 0,
                level3ReferralsPTPUniqueEarning: 0,
                level3ReferralsPTPNonUnique3Earning: 0,
                level3ReferralsPTPNonUnique3Earning: 0,
                level3ReferralsPTPNonUnique3Earning: 0,
                level3ReferralsOfferwallEarning: 0
            };
        },
        methods: {
            test: function () { },
            routerPush: function (location)
            {
                this.$router.push(location);
            }
        },
        mounted()
        {
            this.$store.commit("isLoading", true);

            var data = this.level1Referrals;

            for (var i = 0; i < Object.keys(data).length; i++)
            {
                var totalEarning =
                    (this.$store.getters.faucetSettingValue("Level1").Value / 100) *
                    data[i].LinkShortnerEarning +
                    (this.$store.getters.faucetSettingValue("Level1").Value / 100) *
                    data[i].BonusAdEarning +
                    (this.$store.getters.faucetSettingValue("PTPLevel1").Value / 100) *
                    (data[i].PTPUniqueEarning +
                        data[i].PTPNonUnique1Earning +
                        data[i].PTPNonUnique2Earning +
                        data[i].PTPNonUnique3Earning) +
                    (this.$store.getters.faucetSettingValue("OfferwallLevel1").Value /
                        100) *
                    data[i].OfferwallEarning;
                this.level1ReferralsTotalCredited += totalEarning;
                this.level1ReferralsFaucetClaimEarning += data[i].LinkShortnerEarning;
                this.level1ReferralsBonusAdEarning += data[i].BonusAdEarning;
                this.level1ReferralsPTPUniqueEarning += data[i].PTPUniqueEarning;
                this.level1ReferralsPTPNonUnique1Earning += data[i].PTPNonUnique1Earning;
                this.level1ReferralsPTPNonUnique2Earning += data[i].PTPNonUnique2Earning;
                this.level1ReferralsPTPNonUnique3Earning += data[i].PTPNonUnique3Earning;
                this.level1ReferralsOfferwallEarning += data[i].OfferwallEarning;
            }

            this.level1ReferralsTotalCredited = this.level1ReferralsTotalCredited.toFixed(
                8
            );

            data = this.level2Referrals;

            for (var i = 0; i < Object.keys(data).length; i++)
            {
                var totalEarning =
                    (this.$store.getters.faucetSettingValue("Level2").Value / 100) *
                    data[i].LinkShortnerEarning +
                    (this.$store.getters.faucetSettingValue("Level2").Value / 100) *
                    data[i].BonusAdEarning +
                    (this.$store.getters.faucetSettingValue("PTPLevel2").Value / 100) *
                    (data[i].PTPUniqueEarning +
                        data[i].PTPNonUnique1Earning +
                        data[i].PTPNonUnique2Earning +
                        data[i].PTPNonUnique3Earning) +
                    (this.$store.getters.faucetSettingValue("OfferwallLevel2").Value /
                        100) *
                    data[i].OfferwallEarning;
                this.level2ReferralsTotalCredited += totalEarning;
                this.level2ReferralsFaucetClaimEarning += data[i].LinkShortnerEarning;
                this.level2ReferralsBonusAdEarning += data[i].BonusAdEarning;
                this.level2ReferralsPTPUniqueEarning += data[i].PTPUniqueEarning;
                this.level2ReferralsPTPNonUnique2Earning += data[i].PTPNonUnique2Earning;
                this.level2ReferralsPTPNonUnique2Earning += data[i].PTPNonUnique2Earning;
                this.level2ReferralsPTPNonUnique3Earning += data[i].PTPNonUnique3Earning;
                this.level2ReferralsOfferwallEarning += data[i].OfferwallEarning;
            }

            this.level2ReferralsTotalCredited = this.level2ReferralsTotalCredited.toFixed(
                8
            );
            data = this.level3Referrals;

            for (var i = 0; i < Object.keys(data).length; i++)
            {
                var totalEarning =
                    (this.$store.getters.faucetSettingValue("Level3").Value / 100) *
                    data[i].LinkShortnerEarning +
                    (this.$store.getters.faucetSettingValue("Level3").Value / 100) *
                    data[i].BonusAdEarning +
                    (this.$store.getters.faucetSettingValue("PTPLevel3").Value / 100) *
                    (data[i].PTPUniqueEarning +
                        data[i].PTPNonUnique1Earning +
                        data[i].PTPNonUnique2Earning +
                        data[i].PTPNonUnique3Earning) +
                    (this.$store.getters.faucetSettingValue("OfferwallLevel3").Value /
                        100) *
                    data[i].OfferwallEarning;
                this.level3ReferralsTotalCredited += totalEarning;
                this.level3ReferralsFaucetClaimEarning += data[i].LinkShortnerEarning;
                this.level3ReferralsBonusAdEarning += data[i].BonusAdEarning;
                this.level3ReferralsPTPUniqueEarning += data[i].PTPUniqueEarning;
                this.level3ReferralsPTPNonUnique3Earning += data[i].PTPNonUnique3Earning;
                this.level3ReferralsPTPNonUnique3Earning += data[i].PTPNonUnique3Earning;
                this.level3ReferralsPTPNonUnique3Earning += data[i].PTPNonUnique3Earning;
                this.level3ReferralsOfferwallEarning += data[i].OfferwallEarning;
            }
            this.level3ReferralsTotalCredited = this.level3ReferralsTotalCredited.toFixed(
                8
            );

            this.$store.commit("isLoading", false);
        }
    };
</script>
