using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms;
using Android.Gms.Common;
using Android.Gms.Actions;
using Android.Gms.Ads;
using Android.Gms.Auth;
using Android.Gms.Dynamic;
using Android.Gms.Dynamite;
using Android.Gms.Extensions;
using Android.Gms.Iid;
using Android.Gms.Internal;
using Android.Gms.Location;
using Android.Gms.Security;
using Android.Gms.Tasks;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Java.Lang;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
using Com.Google.Maps.Android.Clustering.View;
using Com.Google.Maps.Android.UI;
using Com.Google.Maps.Android.Clustering;

namespace TK.CustomMap.Droid
{
    public class TKMarkerRenderer : DefaultClusterRenderer
    {
        Context _context;
        GoogleMap _googleMap;
        TKCustomMapRenderer _mapRenderer;
        IconGenerator _iconGenerator;

        public TKMarkerRenderer(Context context, GoogleMap googleMap, ClusterManager clusterManager, TKCustomMapRenderer mapRenderer) :
            base(context, googleMap, clusterManager)
        {
            _context = context;
            _googleMap = googleMap;
            _mapRenderer = mapRenderer;
            _iconGenerator = new IconGenerator(context);
        }

        protected async override void OnBeforeClusterItemRendered(Java.Lang.Object p0, MarkerOptions p1)
        {
            var tkMarker = p0 as TKMarker;

            if (tkMarker == null) return;

            await tkMarker.InitializeMarkerOptionsAsync(p1);
        }
        protected override void OnClusterItemRendered(Java.Lang.Object p0, Marker p1)
        {
            base.OnClusterItemRendered(p0, p1);

            var tkMarker = p0 as TKMarker;

            if (tkMarker == null) return;

            tkMarker.Marker = p1;
        }

        protected async override void OnBeforeClusterRendered(ICluster p0, MarkerOptions p1)
        {
            base.OnBeforeClusterRendered(p0, p1);

            var customPin = _mapRenderer.FormsMap.GetClusteredPin?.Invoke(null, p0.Items.OfType<TKMarker>().Select(i => i.Pin));

            if (customPin == null)
            {
                p1.SetIcon(BitmapDescriptorFactory.FromBitmap(_iconGenerator.MakeIcon(p0.Size.ToString())));
            }
            else
            {
                var tempMarker = new TKMarker(customPin, _context);
                
                await tempMarker.InitializeMarkerOptionsAsync(p1, false);
            }
        }

        protected override void OnClusterRendered(ICluster p0, Marker p1)
        {
            base.OnClusterRendered(p0, p1);

            var tkMarker = p0 as TKMarker;

            if (tkMarker == null) return;

            tkMarker.Marker = p1;
        }
    }
}