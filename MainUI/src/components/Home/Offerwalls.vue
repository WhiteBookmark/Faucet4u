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
                <br />
                <br />

                <p class="font-weight-bold">
                    We pay you for 3 levels of referrals deep. <br />
                    We pay
                    {{ this.$store.getters.faucetSettingValue("OfferwallLevel1").Value }}%
                    to you from all your Level 1 offerwall earnings,<br />
                    {{ this.$store.getters.faucetSettingValue("OfferwallLevel2").Value }}%
                    from all your level 2 referrals offerwall earnings,<br />
                    {{ this.$store.getters.faucetSettingValue("OfferwallLevel3").Value }}%
                    to you from all your level 3 referrals offerwall earnings.<br />
                </p>
                <p class="font-weight-bold text-secondary">
                    *Note: Your offerwall earning including referral offerwall earning is
                    credited to your offerwall balance.
                </p>

                <div class="w-75 mx-auto">
                    <div class="card text-white bg-primary w-100">
                        <div class="card-body">
                            <img src="Images/HitswallLogo.png"
                                 alt="Hitswall"
                                 v-on:click="routerPush('Hitswall')"
                                 width="280"
                                 height="60" />
                        </div>
                    </div>
                    <br />
                    <div class="card text-white bg-primary w-100">
                        <div class="card-body">
                            <img src="Images/PTCWallLogo.png"
                                 alt="PTCWall"
                                 v-on:click="routerPush('PTCWall')"
                                 width="280"
                                 height="60" />
                        </div>
                    </div>
                    <br />
                    <div class="card text-white bg-primary w-100">
                        <div class="card-body">
                            <img src="Images/SkippyAdsLogo.png"
                                 alt="PTCWall"
                                 v-on:click="routerPush('SkippyAds')"
                                 width="280"
                                 height="200" />
                        </div>
                    </div>
                    <br />
                    <div class="card text-white bg-primary w-100">
                        <div class="card-body">
                            <img src="Images/KiwiWallLogo.png"
                                 alt="KiwiWall"
                                 v-on:click="routerPush('KiwiWall')"
                                 width="280"
                                 height="60" />
                        </div>
                    </div>
                </div>
            </mdb-col>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col col="1"> </mdb-col>
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
import { mdbRow, mdbCol, Btn } from "mdbvue";

export default {
  components: { mdbRow, mdbCol, Btn },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      address: this.$store.state.userData[0].FaucetHubBitcoinAddress,
      apiResponse: null,
      ptpURL: `http://localhost:8080/#/?referrer=${
        this.$store.state.userData[0].Username
      }`,
      errorList: null,
      successList: null
    };
  },
  methods: {
    test: function() {},
    routerPush: function(location) {
      this.$router.push(location);
    }
  },
  mounted() {}
};
</script>
